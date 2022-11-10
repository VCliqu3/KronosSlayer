using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCollectionController : MonoBehaviour
{
    private OrbMovementController _orbMovementController;
    private bool canBeCollected = false;

    public LayerMask playerLayer;

    public float collectionRange;
    public float healthAmount;
    public float shieldAmount;

    void Start()
    {
        _orbMovementController = GetComponent<OrbMovementController>();
    }
    void Update()
    {
        if (_orbMovementController.hasStartedOscilating)
        {
            canBeCollected = true;
        }

        OrbCollection();
    }
    
    void OrbCollection()
    {
        if (Physics2D.OverlapCircle(transform.position, collectionRange, playerLayer) && canBeCollected)
        {
            FindObjectOfType<HealthController>().AddHealth(healthAmount);
            FindObjectOfType<HealthController>().AddShield(shieldAmount);
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //Solo se puede recoger si ya ha caido completamente
        {
            canBeCollected = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, collectionRange);
    }
}
