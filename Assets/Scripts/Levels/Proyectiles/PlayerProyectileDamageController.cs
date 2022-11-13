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
            collision.GetComponent<BasicEnemyHealthController>().TakeDamage(damage, shieldPenetration);
            Destroy(gameObject);
            
        }
        else if (collision.CompareTag("KingKronos"))
        {
            collision.GetComponent<KKHealthController>().TakeDamage(damage, shieldPenetration);
            Destroy(gameObject);
        }
    }
}
