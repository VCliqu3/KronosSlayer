using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOrbCollection : MonoBehaviour
{
    private bool canBeCollected = false;

    public LayerMask playerLayer;

    public float collectionRange;
    public float shieldAmount;

    public float timeForCollection;
    private float time;

    void Update()
    {
        time += Time.deltaTime;

        if (time > timeForCollection)
        {
            canBeCollected = true;
        }

        OrbCollection();
    }

    void OrbCollection()
    {
        if (Physics2D.OverlapCircle(transform.position, collectionRange, playerLayer) && canBeCollected)
        {
            HealthController _healthController = FindObjectOfType<HealthController>();

            if(_healthController.shield < _healthController.maxShield)
            {
                _healthController.AddShield(shieldAmount);
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, collectionRange);
    }
}
