using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProyectileDamageController : MonoBehaviour
{
    /*[SerializeField] Player;*/
    public float damage;
    public float shieldPenetration;

    public GameObject ShieldImpactVFX;

    public float scaleVFX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!collision.GetComponent<BasicEnemyHealthController>().isDead)
            {
                BasicEnemyHealthController _BEHealthController = collision.GetComponent<BasicEnemyHealthController>();

                float startingHealth = _BEHealthController.health;

                if (_BEHealthController.shield > 0)
                {
                    CreateFeedbackImpactVFX(ShieldImpactVFX, collision.transform, scaleVFX, 1.2f);
                }

                _BEHealthController.TakeDamage(damage, shieldPenetration);

                if (_BEHealthController.health < startingHealth)
                {
                    //
                }

                Destroy(gameObject);
            }
            
        }
        else if (collision.CompareTag("KingKronos"))
        {
            if (!collision.GetComponent<KKHealthController>().isDead && collision.GetComponent<KKHealthController>().canTakeDamage)
            {
                KKHealthController _KKHealthController = collision.GetComponent<KKHealthController>();

                float startingHealth = _KKHealthController.health;

                if (_KKHealthController.shield > 0)
                {
                    CreateFeedbackImpactVFX(ShieldImpactVFX,collision.transform,scaleVFX,1.2f);
                }

                _KKHealthController.TakeDamage(damage, shieldPenetration);

                if (_KKHealthController.health < startingHealth)
                {
                    //
                }

                Destroy(gameObject);
            }
        }
    }

    public void CreateFeedbackImpactVFX(GameObject feedbackVFX, Transform entHit,float scale, float timeToAutodestroy)
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
