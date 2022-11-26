using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Vector2 BGStartPos;

    public GameObject Player;

    public Vector2 PlayerStartPos;

    public Vector2 PlayerPos;

    public float parallaxEffectX;
    public float parallaxEffectY;

    // Start is called before the first frame update
    void Start()
    {
        BGStartPos = transform.position;

        PlayerStartPos = Player.transform.position;
    }

    void Update()
    {
        transform.position = new Vector2(BGStartPos.x+parallaxEffectX*(Player.transform.position.x-PlayerStartPos.x), BGStartPos.y + parallaxEffectY * (Player.transform.position.y - PlayerStartPos.y ));
    }
}
