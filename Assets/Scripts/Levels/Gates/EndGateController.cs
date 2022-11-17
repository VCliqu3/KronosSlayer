using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGateController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(FindObjectOfType<LevelController>().NextLevelLogic());
        }
    }
}
