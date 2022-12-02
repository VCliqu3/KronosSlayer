using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProyectileDamageController : MonoBehaviour
{
    public float damage;
    public float shieldPenetration;

    public GameObject ShieldImpactVFX;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!collision.GetComponent<HealthController>().invincibilityEnabled) 
            {
                if(collision.GetComponent<HealthController>().shield > 0)
                {
                    GameObject ShImpVFX = Instantiate(ShieldImpactVFX, transform.position, transform.rotation);

                    AvoidParentRotation _APR = ShImpVFX.GetComponent<AvoidParentRotation>();

                    _APR.hitInitialPos = transform.position;
                    _APR.entityHitTranform = collision.transform;
                    _APR.CalculateOffsetVector();

                    Destroy(ShImpVFX, 1.2f);
                }

                collision.GetComponent<HealthController>().TakeDamage(damage, shieldPenetration);
                Destroy(gameObject);
            }
        }
    }
}
