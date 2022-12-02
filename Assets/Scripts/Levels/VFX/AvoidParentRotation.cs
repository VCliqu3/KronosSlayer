using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidParentRotation : MonoBehaviour
{
    public Transform entityHitTranform;
    public Vector3 hitInitialPos;
    private Vector3 offsetVector;
    // Update is called once per frame

    void Update()
    {
        if(entityHitTranform != null)
        {
            transform.position = new Vector2(offsetVector.x + entityHitTranform.position.x, offsetVector.y + entityHitTranform.position.y);
        }
    }

    public void CalculateOffsetVector()
    {
        offsetVector = hitInitialPos - entityHitTranform.position;
    }
}
