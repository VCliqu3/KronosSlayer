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
    private Animator _animator;

    public float timeHurting = 0.5f;
    public bool isHurting = false;

    private HUDController _HUDController;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _basicEnemyHealthBarController = GetComponent<BasicEnemyHealthBarController>();
        _basicEnemyScoreController = GetComponent<BasicEnemyScoreController>();
        _HUDController = FindObjectOfType<HUDController>();

        health = maxHealth;
        shield = maxShield;
    }

    public void TakeDamage(float damage, float shieldPenetration)
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
            health = 0;
            KillEnemy();
        }
        else
        {
            health = auxHealth;
            HurtEnemy();
        }

        _basicEnemyHealthBarController.SetHealthBar();
        _basicEnemyHealthBarController.SetShieldBar();
    }

    void HurtEnemy()
    {   
        _animator.SetTrigger("GetHurt"); //Trigger para animacion GetHurt
    }

    void KillEnemy()
    {
        FindObjectOfType<ScoreController>().AddScoreInCurrentLevel(_basicEnemyScoreController.enemyScore);
        _HUDController.SetScoreText();
        Destroy(gameObject);
    }
}
