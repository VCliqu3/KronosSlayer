using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : MonoBehaviour
{
    private RectTransform _rectTransform;
    public float speedX;
    public float speedY;

    private Vector2 initialPos;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        initialPos = _rectTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        LineMovement();
    }

    void LineMovement()
    {
        float displacementX = time * speedX;
        float displacementY = time * speedY;

        _rectTransform.position = new Vector2(initialPos.x + displacementX, initialPos.y + displacementY);
    }
}
