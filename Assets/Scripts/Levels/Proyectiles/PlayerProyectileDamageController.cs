using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProyectileDamageController : MonoBehaviour
{
    /*[SerializeField] Player;*/
    public float damage;
    public float shieldPenetration;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!collision.GetComponent<BasicEnemyHealthController>().isDead)
            {
                collision.GetComponent<BasicEnemyHealthController>().TakeDamage(damage, shieldPenetration);
                Destroy(gameObject);
            }
            
        }
        else if (collision.CompareTag("KingKronos"))
        {
            if (!collision.GetComponent<KKHealthController>().isDead)
            {
                collision.GetComponent<KKHealthController>().TakeDamage(damage, shieldPenetration);
                Destroy(gameObject);
            }
        }
    }
}
