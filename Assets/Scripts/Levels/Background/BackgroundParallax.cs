using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Vector2 BGPos;

    public Vector2 BGStartPos;

    public GameObject Player;

    public Vector2 PlayerStartPos;

    public Vector2 PlayerPos;

    public float parallaxEffectX;
    public float parallaxEffectY;

    // Start is called before the first frame update
    void Start()
    {
        BGPos = transform.position;
        BGStartPos = BGPos;
    }

    void Update()
    {
        BGPos = transform.position;
    }
}
