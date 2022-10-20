using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProyectileDamageController : MonoBehaviour
{
    public float damage;
    public float shieldPenetration;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!collision.GetComponent<HealthController>().invincibilityEnabled) 
            {
                collision.GetComponent<HealthController>().TakeDamage(damage, shieldPenetration);
                Destroy(gameObject);
            }
        }
    }
}
