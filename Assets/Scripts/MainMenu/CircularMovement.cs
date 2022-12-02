using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    private RectTransform _rectTransform;
    public float speed;
    public float limitX;
    public float limitY;

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
        CircleMovement();
    }

    void CircleMovement()
    {
        float displacementX = Mathf.Cos(time * speed) * limitX;
        float displacementY = Mathf.Sin(time * speed) * limitY;

        _rectTransform.position = new Vector2(initialPos.x + displacementX, initialPos.y + displacementY);
    }
}
