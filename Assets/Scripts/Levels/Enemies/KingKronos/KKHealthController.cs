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
    public float damageAccumulationEmptyRate = 7.5f;

    public float timeToAccumulate1Attack = 1f;
    public float damageAccumulationMultiplier = 1f;
    public float enragedDamageAccumulationMultiplier = 1.5f;

    public float timeToEmptyDamageAccumulationBar = 3f;
    public float timeToAccumulateAfterEmpty = 1f;

    public float shieldAbsorption = 0.8f; //Expresado en porcentaje (0 a 1), teoricamente puede ser mayor a 1 y menor a 0 


    private KKHUDController _KKHUDController;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;

    private KKMovementController _KKMovementController;
    private KKAttackController _KKAttackController;
    private KKDashController _KKDashController;
    private KKJumpAttackController _KKJumpAttackController;
    private KKTPController _KKTPController;
    private DashShadowsController _dashShadowsController;

    private BasicEnemyScoreController _basicEnemyScoreController;
    private HUDController _HUDController;

    public float damageReduction = 0;
    public bool canTakeDamage = true;
    public bool hurtEnable = true;
    public float timeHurting = 1f;
    public bool isHurting = false;
    public bool isDead = false;

    public float deathFallImpulse = 15f;
    public float deathImpulse = 6f;

    public bool isEnraged = false;
    public bool onEnrageAnim = false;

    public float timeFadeAfterDeath = 3f;

    public TrailRenderer _trailRenderer;
    public PhysicsMaterial2D bouncyMaterial;

    private SpriteRenderer _spriteRenderer;
    private Material _material;
    public float timeToChangeColor = 2f;
    public Color enragedColor;
    public Color enragedShadowsColor;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _KKHUDController = FindObjectOfType<KKHUDController>();

        _KKMovementController = GetComponent<KKMovementController>();
        _KKAttackController = GetComponent<KKAttackController>();
        _KKJumpAttackController = GetComponent<KKJumpAttackController>();
        _KKDashController = GetComponent<KKDashController>();
        _KKTPController = GetComponent<KKTPController>();
        _dashShadowsController = GetComponent<DashShadowsController>();

        _basicEnemyScoreController = GetComponent<BasicEnemyScoreController>();
        _HUDController = FindObjectOfType<HUDController>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;


        health = maxHealth;
        shield = maxShield;

        canTakeDamage = false;
        damageReduction = 0;
    }

    public void TakeDamage(float damage, float shieldPenetration)
    {
        damage *= (1-damageReduction);

        if (!isDead && damageReduction<1 && canTakeDamage)
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
                    StartCoroutine(HurtEnemy());
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

    IEnumerator HurtEnemy()
    {
        isHurting = true; //Bool para trigger colliders se vuelve true
        canTakeDamage = false;

        //_animator.SetTrigger("GetHurt"); //Trigger para animacion GetHurt
        _animator.SetLayerWeight(1, 1); //GetHurtBlinkingAnimation Activada

        yield return new WaitForSeconds(timeHurting);

        _animator.SetLayerWeight(1, 0); //GetHurtBlinkingAnimation Desactivada

        isHurting = false; //Bool para trigger colliders regresa a false

        if (!onEnrageAnim)
        {
            canTakeDamage = true;
        }
    }

    IEnumerator KillEnemy()
    {
        health = 0;
        isDead = true;

        _trailRenderer.emitting = false;

        _rigidbody2D.gravityScale = 1;
        _KKMovementController.Stop();
        _KKMovementController.StopOnY();

        _KKDashController.StopAllCoroutines();
        _KKJumpAttackController.StopAllCoroutines();
        _KKTPController.StopAllCoroutines();

        FindObjectOfType<ScoreController>().AddScoreInCurrentLevel(_basicEnemyScoreController.enemyScore);
        FindObjectOfType<ScoreController>().AddEnemiesKilledInCurrentLevel(1);

        _HUDController.SetScoreText();

        _rigidbody2D.sharedMaterial = bouncyMaterial;

        if (!_KKMovementController.isGrounded)
        {
            _animator.Play("FallToDie");

            _rigidbody2D.AddForce(new Vector2(0, -deathFallImpulse), ForceMode2D.Impulse);

            while (!_KKMovementController.isGrounded)
            {      
                yield return null;
            }
        }
        else 
        { 
            _animator.Play("Death");
        }

        _KKMovementController.KKCanvasAnimator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(timeFadeAfterDeath);
        _animator.SetTrigger("FadeOut");
    }

    public void DestroyEnemy()
    {
        if (FindObjectOfType<BossBattleController>() != null)
        {
            BossBattleController _kkBattleController = FindObjectOfType<BossBattleController>();
            _kkBattleController.StartCoroutine(_kkBattleController.EndBossBattle());
        }

        Destroy(gameObject);
    }
    
    IEnumerator AccumulateDamage(float damage)
    {
        canAccumulateDamage = false;
        float auxDamage = damage * damageAccumulationMultiplier;

        while(auxDamage > 0 && damageAccumulatedCounter<= damageAccumulationLimit)
        {
            damageAccumulatedCounter += Time.deltaTime * damage / timeToAccumulate1Attack; 
            auxDamage -= Time.deltaTime * damage / timeToAccumulate1Attack;

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
            damageAccumulatedCounter -= Time.deltaTime * damageAccumulationLimit/ timeToEmptyDamageAccumulationBar;
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

        StartCoroutine(ChangeColor(enragedColor));
        _dashShadowsController._color = enragedShadowsColor;

        damageAccumulationMultiplier = enragedDamageAccumulationMultiplier; //Cambian las estadisticas de las habilidades KingKronos

        _KKMovementController.runSpeed = _KKMovementController.enragedRunSpeed;

        _KKAttackController.attackDuration = _KKAttackController.enragedAttackDuration;
        _KKAttackController.attackDamage = _KKAttackController.enragedAttackDamage;
        _KKAttackController.attackShieldPenetration = _KKAttackController.enragedAttackShieldPenetration;

        _KKDashController.timeChargingDash = _KKDashController.enragedTimeChargingDash;
        _KKDashController.dashForce = _KKDashController.enragedDashForce;
        _KKDashController.dashCooldown = _KKDashController.enragedDashCooldown;
        _KKDashController.dashAttackDamage = _KKDashController.enragedDashAttackDamage;
        _KKDashController.dashAttackShieldPenetration = _KKDashController.enragedDashAttackShieldPenetration;

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
        _KKTPController.downImpulse = _KKTPController.enragedDownImpulse;
        _KKTPController.timeStayingUp = _KKTPController.enragedTimeStayingUp;
    }

    IEnumerator ChangeColor(Color targetColor)
    {
        float t = 0;
        Color initialColor = _spriteRenderer.color;

        while (t < timeToChangeColor)
        {
            t += Time.deltaTime;
            _material.color = Color.Lerp(initialColor, targetColor, t / timeToChangeColor);
            yield return null;
        }
    }

    public void DeathImpulse()
    {
        _rigidbody2D.AddForce(new Vector2(0, deathImpulse), ForceMode2D.Impulse);
    }
}
