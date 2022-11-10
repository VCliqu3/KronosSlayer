using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrbController : MonoBehaviour
{
    public float healthAmount;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<HealthController>().AddHealth(healthAmount);
            Destroy(gameObject);
        }
    }
}
