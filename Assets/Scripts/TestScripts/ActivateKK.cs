using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateKK : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            if(FindObjectOfType<KKMovementController>() != null)
            {
                if (!FindObjectOfType<KKMovementController>().isActivated)
                {
                    FindObjectOfType<KKMovementController>()._animator.SetTrigger("OPAnim");
                }
            }
        }
    }
}
