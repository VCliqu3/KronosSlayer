using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKHealthController : MonoBehaviour
{
    public float maxHealth = 15;
    public float health;

    public float maxShield = 20;
    public float shield;

    public bool canAccumulateDamage = true;
    public float damageAccumulationLimit = 20;
    public float damageAccumulatedCounter = 0;
    public float damageAccumulationSpeedRate = 1f;
    public float damageAccumulationEmptyRate = 7.5f;

    public float damageAccumulationMultiplier = 1f;
    public float enragedDamageAccumulationMultiplier = 1.5f;

    public float timeToAccumulateAfterEmpty = 1f;

    public float shieldAbsorption = 0.8f; //Expresado en porcentaje (0 a 1), teoricamente puede ser mayor a 1 y menor a 0 
    public float damageReduction = 0;

    private KKHUDController _KKHUDController;
    private Animator _animator;
    private BoxCollider2D _boxCollider2D;

    private KKAttackController _KKAttackController;
    private KKDashController _KKDashController;
    private KKJumpAttackController _KKJumpAttackController;
    private KKTPController _KKTPController;

    public bool hurtEnable = true;
    public float timeHurting = 0.5f;
    public bool isHurting = false;
    public bool isDead = false;

    public bool isEnraged = false;
    public bool onEnrageAnim = false;

    public float timeFadeAfterDeath = 3f;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _KKHUDController = FindObjectOfType<KKHUDController>();

        _KKAttackController = GetComponent<KKAttackController>();
        _KKJumpAttackController = GetComponent<KKJumpAttackController>();
        _KKDashController = GetComponent<KKDashController>();
        _KKTPController = GetComponent<KKTPController>();

        health = maxHealth;
        shield = maxShield;

        damageReduction = 1;
    }

    public void TakeDamage(float damage, float shieldPenetration)
    {
        damage *= (1-damageReduction);

        if (!isDead && damageReduction<1)
        {
            float damageShieldWouldTake, damageHealthWouldTake;
            float resultingShAb;
            float auxHealth = health; //Health puede ser negativo al recibir daño, por ello se declara una variable auxiliar

            resultingShAb = shieldAbsorption - shieldPenetration; //Absorcion de escudo resultante

            resultingShAb = resultingShAb < 0 ? 0 : resultingShAb; //Por si el resultado es <0
            resultingShAb = resultingShAb > 1 ? 1 : resultingShAb; //Por si el resultado es >1

            damageShieldWouldTake = damage * (resultingShAb); //Daño que el escudo fuera a recibir
            damageHealthWouldTake = damage * (1 - resultingShAb); //Daño que la vida fuera a recibir

            if (damageShieldWouldTake > shield)
            {
                auxHealth -= damageHealthWouldTake + (damageShieldWouldTake - shield);
                shield = 0;

            }
            else
            {
                auxHealth -= damageHealthWouldTake;
                shield -= damageShieldWouldTake;
            }

            if (auxHealth <= 0.1f) //Para resolver bug de float y que no se muestre una cantidad imperceptible en la barra de vida
            {
                StartCoroutine(KillEnemy());
            }
            else
            {
                health = auxHealth;

                if (hurtEnable)
                {
                    HurtEnemy();
                }
            }

            if (canAccumulateDamage)
            {
                StartCoroutine(AccumulateDamage(damage));
            }

            _KKHUDController.SetHealthBar();
            _KKHUDController.SetShieldBar();
        }
    }

    void HurtEnemy()
    {
        _animator.SetTrigger("GetHurt"); //Trigger para animacion GetHurt
    }

    IEnumerator KillEnemy()
    {
        health = 0;
        isDead = true;

        FindObjectOfType<ScoreController>().AddEnemiesKilledInCurrentLevel(1);

        _animator.SetTrigger("Death");
        //_boxCollider2D.enabled = false;

        yield return new WaitForSeconds(timeFadeAfterDeath);

        _animator.SetTrigger("FadeOut");
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    
    IEnumerator AccumulateDamage(float damage)
    {
        canAccumulateDamage = false;
        float auxDamage = damage * damageAccumulationMultiplier;

        while(auxDamage > 0 && damageAccumulatedCounter<= damageAccumulationLimit)
        {
            damageAccumulatedCounter += Time.deltaTime * damageAccumulationSpeedRate;
            auxDamage -= Time.deltaTime * damageAccumulationSpeedRate;

            _KKHUDController.SetDamageAccumulationBar();
            yield return null;
        }

        damageAccumulatedCounter = damageAccumulatedCounter > damageAccumulationLimit ? damageAccumulationLimit : damageAccumulatedCounter;
        _KKHUDController.SetDamageAccumulationBar();

        if (damageAccumulatedCounter < damageAccumulationLimit)
        {
            canAccumulateDamage = true;
        }
    }

    public IEnumerator EmptyDamageAccumulated()
    {
        while (damageAccumulatedCounter > 0)
        {
            damageAccumulatedCounter -= Time.deltaTime * damageAccumulationEmptyRate;
            _KKHUDController.SetDamageAccumulationBar();

            yield return null;
        }

        damageAccumulatedCounter = damageAccumulatedCounter < 0 ? 0 : damageAccumulatedCounter;
        _KKHUDController.SetDamageAccumulationBar();

        yield return new WaitForSeconds(timeToAccumulateAfterEmpty);

        canAccumulateDamage = true;
    }

    public void CallEmptyDamageAccumulated()
    {
        StartCoroutine(EmptyDamageAccumulated());
    }
    
    public void Enrage()
    {
        isEnraged = true;

        damageAccumulationMultiplier = enragedDamageAccumulationMultiplier; //Cambian las estadisticas de las habilidades KingKronos

        _KKAttackController.attackDuration = _KKAttackController.enragedAttackDuration;
        _KKAttackController.attackDamage = _KKAttackController.enragedAttackDamage;
        _KKAttackController.attackShieldPenetration = _KKAttackController.enragedAttackShieldPenetration;

        _KKDashController.timeChargingDash = _KKDashController.enragedTimeChargingDash;
        _KKDashController.dashForce = _KKDashController.enragedDashForce;
        _KKDashController.dashCooldown = _KKDashController.enragedDashCooldown;

        _KKJumpAttackController.jumpAttackDamage = _KKJumpAttackController.enragedJumpAttackDamage;
        _KKJumpAttackController.jumpAttackShieldPenetration = _KKJumpAttackController.enragedJumpAttackShieldPenetration;
        _KKJumpAttackController.fallSpeed = _KKJumpAttackController.enragedFallSpeed;
        _KKJumpAttackController.timeChargingJump = _KKJumpAttackController.enragedTimeChargingJump;
        _KKJumpAttackController.jumpCooldown = _KKJumpAttackController.enragedJumpCooldown;
        _KKJumpAttackController.parabolaPercentage = _KKJumpAttackController.enragedParabolaPercentage;
        _KKJumpAttackController.jumpAngle = _KKJumpAttackController.enragedJumpAngle;

        _KKTPController.TPAttackDamage = _KKTPController.enragedTPAttackDamage;
        _KKTPController.TPAttacShieldPenetration = _KKTPController.enragedTPAttacShieldPenetration;
        _KKTPController.timeChargingTP = _KKTPController.enragedtimeChargingTP;
        _KKTPController.downImpulse = _KKTPController.enrageddownImpulse;
        _KKTPController.timeStayingUp = _KKTPController.enragedTimeStayingUp;
    }
}
