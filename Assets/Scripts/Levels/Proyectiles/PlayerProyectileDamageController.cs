using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProyectileDamageController : MonoBehaviour
{
    /*[SerializeField] Player;*/
    public float damage;
    public float shieldPenetration;

    public GameObject ShieldImpactVFX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!collision.GetComponent<BasicEnemyHealthController>().isDead)
            {
                if (collision.GetComponent<BasicEnemyHealthController>().shield > 0)
                {
                    GameObject ShImpVFX = Instantiate(ShieldImpactVFX, transform.position, transform.rotation);
                    ShImpVFX.transform.parent = collision.transform;
                    Destroy(ShImpVFX, 1.2f);
                }

                collision.GetComponent<BasicEnemyHealthController>().TakeDamage(damage, shieldPenetration);
                Destroy(gameObject);
            }
            
        }
        else if (collision.CompareTag("KingKronos"))
        {
            if (!collision.GetComponent<KKHealthController>().isDead && collision.GetComponent<KKHealthController>().canTakeDamage)
            {
                if (collision.GetComponent<KKHealthController>().shield > 0)
                {
                    GameObject ShImpVFX = Instantiate(ShieldImpactVFX, transform.position, transform.rotation);
                    ShImpVFX.transform.parent = collision.transform;
                    Destroy(ShImpVFX, 1.2f);
                }
                collision.GetComponent<KKHealthController>().TakeDamage(damage, shieldPenetration);
                Destroy(gameObject);
            }
        }
    }
}
