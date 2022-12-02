using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProyectileDamageController : MonoBehaviour
{
    public float damage;
    public float shieldPenetration;

    public GameObject ShieldImpactVFX;

    public float scaleVFX;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!collision.GetComponent<HealthController>().invincibilityEnabled) 
            {
                HealthController _HealthController = collision.GetComponent<HealthController>();

                float startingHealth = _HealthController.health;

                if(_HealthController.shield > 0)
                {
                    CreateFeedbackImpactVFX(ShieldImpactVFX, collision.transform,scaleVFX, 1.2f);
                }

                _HealthController.TakeDamage(damage, shieldPenetration);

                if (_HealthController.health < startingHealth)
                {
                    //
                }

                Destroy(gameObject);
            }
        }
    }

    public void CreateFeedbackImpactVFX(GameObject feedbackVFX, Transform entHit, float scale, float timeToAutodestroy)
    {
        GameObject fVFX = Instantiate(feedbackVFX, transform.position, transform.rotation);

        AvoidParentRotation _APR = fVFX.GetComponent<AvoidParentRotation>();

        fVFX.transform.localScale = fVFX.transform.localScale * scale;

        _APR.hitInitialPos = transform.position;
        _APR.entityHitTranform = entHit;
        _APR.CalculateOffsetVector();

        Destroy(fVFX, timeToAutodestroy);
    }
}
