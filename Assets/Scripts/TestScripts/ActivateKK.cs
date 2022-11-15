using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateKK : MonoBehaviour
{
    public GameObject KingKronos;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            KingKronos.SetActive(true);
        }
    }
}
