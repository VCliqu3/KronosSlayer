using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseButonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    static public bool mouseOnPauseButton = false;

    // Start is called before the first frame update
    void Start()
    {
        mouseOnPauseButton = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOnPauseButton = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOnPauseButton = false;
    }
}
