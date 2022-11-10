using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOrbController : MonoBehaviour
{
    public float shieldAmount;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<HealthController>().AddShield(shieldAmount);
            Destroy(gameObject);
        }
    }
}
