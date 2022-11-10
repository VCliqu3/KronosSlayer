using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyDropsController : MonoBehaviour
{
    public float shieldOrbDropOdds = 0.2f;
    public float healthOrbDropOdds = 0.2f;

    public bool willDropShieldOrb =false;
    public bool willDropHealthOrb = false;

    public GameObject shieldOrb;
    public GameObject healthOrb;

    private HealthController _playerHealthController;
    // Start is called before the first frame update
    void Start()
    {
        _playerHealthController = FindObjectOfType<HealthController>();

        float random1 = Random.Range(0f, 1f);
        float random2 = Random.Range(0f, 1f);

        if (shieldOrbDropOdds >= random1)
        {
            willDropShieldOrb = true;
        }

        if (healthOrbDropOdds >= random2)
        {
            willDropHealthOrb = true;
        }

        if(willDropHealthOrb && willDropShieldOrb)
        {
            if(_playerHealthController.health != _playerHealthController.maxHealth)
            {
                willDropShieldOrb = false; //Si le falta vida al player, dropea el orbe de vida
            }
            else 
            {
                willDropHealthOrb = false; //Si la vida esta completa, dropea el orbe de escudo
            }
        }
    } 
    public void BasicEnemyDrops()
    {
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
