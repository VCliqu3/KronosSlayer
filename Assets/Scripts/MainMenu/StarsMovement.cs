using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsMovement : MonoBehaviour
{
    public float speedX,speedY;
    private RectTransform _rectTransform;
    private Rigidbody2D _rigidbody2D;
    private Vector2 position;
    private Vector2 _size;

    private float limitX;
    private float limitY;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rectTransform = GetComponent<RectTransform>();
        _size = _rectTransform.sizeDelta;
        limitX = _size.x / 3;
        limitY = _size.y / 3;
    }
    // Update is called once per frame
    void Update()
    {
        _rigidbody2D.velocity = new Vector2(speedX, speedY);
        RepositionStars();
    }

    void RepositionStars()
    {
        position = _rectTransform.position;

        if (position.x >= limitX)
        {
            position.x -= limitX;
            _rectTransform.position = position;
        }
        if (position.x <= -limitX)
        {
            position.x += limitX;
            _rectTransform.position = position;
        }
        if (position.y >= limitY)
        {
            position.y -= limitY;
            _rectTransform.position = position;
        }
        if(position.y <= -limitY)
        {
            position.y += limitY;
            _rectTransform.position = position;
        }
    }
}
