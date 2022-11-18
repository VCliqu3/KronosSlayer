using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButonController : MonoBehaviour
{
    static public bool mouseOnPauseButton = false;
    // Start is called before the first frame update
    void Start()
    {
        mouseOnPauseButton = false;
    }

    void OnMouseEnter()
    {
        mouseOnPauseButton = true;
        CursorController.onGameplay = false;
        Debug.Log("On");
    }

    void OnMouseExit()
    {
        mouseOnPauseButton = false;
        CursorController.onGameplay = true;
        Debug.Log("Out");
    }
}
