using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrbCollection : MonoBehaviour
{
    private bool canBeCollected = false;

    public LayerMask playerLayer;

    public float collectionRange;
    public float healthAmount;

    public float timeForCollection;
    private float time;

    void Update()
    {
        time += Time.deltaTime;

        if(time > timeForCollection)
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

            if (_healthController.health < _healthController.maxHealth)
            {
                _healthController.AddHealth(healthAmount);
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, collectionRange);
    }
}
