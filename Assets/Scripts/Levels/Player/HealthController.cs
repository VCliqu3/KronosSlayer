using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    public float maxHealth = 25;
    public float health;

    public float maxShield = 30;
    public float shield;

    public float shieldAbsorption = 0.8f; //Expresado en porcentaje (0 a 1), teoricamente puede ser mayor a 1 y menor a 0 

    public float timeHurting = 0.5f;
    public float invencibilitySeconds = 3f; //Como minimo 0.5s

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private MovementController _movementController;

    public bool isHurting = false;
    public bool hasBeenDamaged = false;
    public bool playerHasDied = false;

    public bool invincibilityEnabled = false;

    public float timeToPopUpDeathPanel = 2f;

    private HUDController _HUDController;

    public PhysicsMaterial2D bouncyMaterial;
    public float deathImpulseX = 2f;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _movementController = GetComponent<MovementController>();

        _HUDController = FindObjectOfType<HUDController>();

        health = maxHealth;
        shield = maxShield;
    }

    void FixedUpdate()
    {
        EnableDisableInvincibility();
    }

    void EnableDisableInvincibility()
    {
        if(hasBeenDamaged || (_movementController.isDashing && _movementController.dashThroughProyectiles))
        {
            invincibilityEnabled = true;
        }
        else
        {
            invincibilityEnabled = false;
        }
    }

    public void TakeDamage(float damage, float shieldPenetration)
    {
        
        if (!invincibilityEnabled)
        {
            float damageShieldWouldTake, damageHealthWouldTake;
            float resultingShAb;
            float auxHealth = health; //Health puede ser negativo al recibir da�o, por ello se declara una variable auxiliar

            resultingShAb = shieldAbsorption - shieldPenetration; //Absorcion de escudo resultante

            resultingShAb = resultingShAb < 0 ? 0 : resultingShAb; //Por si el resultado es <0
            resultingShAb = resultingShAb > 1 ? 1 : resultingShAb; //Por si el resultado es >1

            damageShieldWouldTake = damage * (resultingShAb); //Da�o que el escudo fuera a recibir
            damageHealthWouldTake = damage * (1 - resultingShAb); //Da�o que la vida fuera a recibir

            if (damageShieldWouldTake>shield)
            {
                auxHealth -= damageHealthWouldTake + (damageShieldWouldTake - shield);
                shield = 0;
                
            }
            else
            {
                auxHealth -= damageHealthWouldTake;
                shield -= damageShieldWouldTake;         
            }
      
            if(auxHealth <= 0.1f) //Para resolver bug de float y que no se muestre una cantidad imperceptible en la barra de vida
            {
                health = 0;
                StartCoroutine(KillPlayer());
            }
            else
            {
                health = auxHealth;
                StartCoroutine(HurtPlayer());

            }

            _HUDController.SetHealthBar();
            _HUDController.SetShieldBar();
        }
    }

    IEnumerator HurtPlayer()
    {
        Physics2D.IgnoreLayerCollision(3, 7); //Ignora colision entre la capa 3 (Player) y la capa 7 (Hazard) (Invulnerabilidad) (Solo Colisiones, no triggers)

        isHurting = true; //Bool para trigger colliders se vuelve true
        hasBeenDamaged = true;

        _animator.SetTrigger("GetHurt"); //Trigger para animacion GetHurt
        _animator.SetLayerWeight(1, 1); //GetHurtBlinkingAnimation Activada

        yield return new WaitForSeconds(timeHurting);

        isHurting = false; //Bool para trigger colliders regresa a false

        yield return new WaitForSeconds(invencibilitySeconds - timeHurting); //Espera 2.5s mas 

        Physics2D.IgnoreLayerCollision(3, 7, false); //Activa colision entre la capa 3 y 7 (Desactiva Invulnerabilidad)
        _animator.SetLayerWeight(1, 0); //GetHurtBlinkingAnimation Desactivada
        hasBeenDamaged = false;
    }

    public void AddHealth(float healthAmount)
    {
        float auxHealth = health;
        auxHealth += healthAmount;

        health = auxHealth > maxHealth ? maxHealth : auxHealth;

        _HUDController.SetHealthBar();
    }
    public void LoseHealth(float healthAmount)
    {
        float auxHealth = health;
        auxHealth -= healthAmount;      

        if (auxHealth <= 0.1f) //Para resolver bug de float y que no se muestre una cantidad imperceptible en la barra de vida
        {
            health = 0;
            StartCoroutine(KillPlayer());
        }
        else
        {
            health = auxHealth;
        }

        _HUDController.SetHealthBar();
    }

    IEnumerator KillPlayer()
    {
        PauseController.canPauseGame = false;
        _animator.SetTrigger("Death");
        playerHasDied = true;

        Physics2D.IgnoreLayerCollision(9, 10); //Collisiones contra Enemigos
        Physics2D.IgnoreLayerCollision(11, 10); //Collisiones contra KingKronos
        gameObject.layer = 10;

        _rigidbody2D.sharedMaterial = bouncyMaterial;

        if (_movementController.playerFacingRight)
        {
            _rigidbody2D.AddForce(new Vector2(deathImpulseX, 0), ForceMode2D.Impulse);
        }
        else
        {
            _rigidbody2D.AddForce(new Vector2(-deathImpulseX, 0), ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(timeToPopUpDeathPanel);

        FindObjectOfType<LevelController>().ActivateDeathPanel();        
    }

    public void AddShield(float shieldAmount)
    {
        float auxShield = shield;
        auxShield += shieldAmount;

        shield = auxShield > maxShield ? maxShield : auxShield;

        _HUDController.SetShieldBar();
    }

    public void LoseShield(float shieldAmount)
    {
        float auxShield = shield;
        auxShield -= shieldAmount;

        shield = auxShield < 0 ? 0 : auxShield;

        _HUDController.SetShieldBar();
    }

    public void FullHealth()
    {
        health = maxHealth;

        _HUDController.SetHealthBar();
    }

    public void FullShield()
    {
        shield = maxShield;

        _HUDController.SetShieldBar();
    }
}
