using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyDropsController : MonoBehaviour
{
    public float shieldOrbDropOdds = 0.2f;
    public float healthOrbDropOdds = 0.2f;

    private bool willDropShieldOrb = false;
    private bool willDropHealthOrb = false;

    public GameObject shieldOrb;
    public GameObject healthOrb;

    private HealthController _playerHealthController;
 
    public void BasicEnemyDrops()
    {
        _playerHealthController = FindObjectOfType<HealthController>();

        float randomNumberShield = Random.Range(0f, 1f);
        float randomNumberHealth = Random.Range(0f, 1f);

        if (shieldOrbDropOdds >= randomNumberShield)
        {
            willDropShieldOrb = true;
        }

        if (healthOrbDropOdds >= randomNumberHealth)
        {
            willDropHealthOrb = true;
        }

        if (willDropHealthOrb && willDropShieldOrb)
        {
            if (_playerHealthController.health != _playerHealthController.maxHealth)
            {
                willDropShieldOrb = false; //Si le falta vida al player, dropea el orbe de vida
            }
            else
            {
                willDropHealthOrb = false; //Si la vida esta completa, dropea el orbe de escudo
            }
        }

        if (willDropShieldOrb)
        {
            DropShieldOrb();
        }

        if (willDropHealthOrb)
        {
            DropHealthOrb();
        }
    }

    public void DropShieldOrb()
    {
        Instantiate(shieldOrb, transform.position, transform.rotation);
    }

    public void DropHealthOrb()
    {
        Instantiate(healthOrb, transform.position, transform.rotation);
    }
}
