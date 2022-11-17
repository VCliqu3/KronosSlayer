using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyHealthController : MonoBehaviour
{    
    public float maxHealth = 15;
    public float health;

    public float maxShield = 20;
    public float shield;

    public float shieldAbsorption = 0.8f; //Expresado en porcentaje (0 a 1), teoricamente puede ser mayor a 1 y menor a 0 

    private BasicEnemyHealthBarController _basicEnemyHealthBarController;
    private BasicEnemyScoreController _basicEnemyScoreController;
    private BasicEnemyDropsController _basicEnemyDropsController;
    private Animator _animator;
    private BoxCollider2D _boxCollider2D;

    public bool hurtEnable = true;
    public float timeHurting = 0.5f;
    public bool isHurting = false;
    public bool isDead = false;

    private HUDController _HUDController;

    public float timeFadeAfterDeath = 3f;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _basicEnemyHealthBarController = GetComponent<BasicEnemyHealthBarController>();
        _basicEnemyScoreController = GetComponent<BasicEnemyScoreController>();
        _basicEnemyDropsController = GetComponent<BasicEnemyDropsController>();
        _HUDController = FindObjectOfType<HUDController>();

        health = maxHealth;
        shield = maxShield;
    }

    public void TakeDamage(float damage, float shieldPenetration)
    {
        if (!isDead)
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

                if (shield <=0) //if hurtEnable
                {
                    HurtEnemy();
                }
            }

            _basicEnemyHealthBarController.SetHealthBar();
            _basicEnemyHealthBarController.SetShieldBar();
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
        gameObject.tag = "DeadEnemy";

        FindObjectOfType<ScoreController>().AddScoreInCurrentLevel(_basicEnemyScoreController.enemyScore);
        FindObjectOfType<ScoreController>().AddEnemiesKilledInCurrentLevel(1);

        _HUDController.SetScoreText();

        _basicEnemyDropsController.BasicEnemyDrops();

        _animator.SetTrigger("Death");
        _boxCollider2D.enabled = false;

        yield return new WaitForSeconds(timeFadeAfterDeath);

        _animator.SetTrigger("FadeOut");
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
