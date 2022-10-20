using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProyectileDamageController : MonoBehaviour
{
    public float damage;
    public float shieldPenetration;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<BasicEnemyHealthController>().TakeDamage(damage, shieldPenetration);
            Destroy(gameObject);
            
        }
    }
}
