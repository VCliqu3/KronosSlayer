using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOrbController : MonoBehaviour
{
    private OrbMovementController _orbMovementController;
    private bool canBeCollected = false;

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
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canBeCollected) //Solo se puede recoger si ya ha caido completamente
        {
            collision.GetComponent<HealthController>().AddShield(shieldAmount);
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
}
