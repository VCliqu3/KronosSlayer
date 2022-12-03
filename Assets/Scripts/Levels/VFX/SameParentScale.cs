using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameParentScale : MonoBehaviour
{
    private Transform parentTransform;
    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent.transform;   
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = parentTransform.localScale;
    }
}
