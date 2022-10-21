using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDamageController : MonoBehaviour
{
    public float damage;
    public float shieldPenetration;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<HealthController>().TakeDamage(damage, shieldPenetration);
        }
    }

}
