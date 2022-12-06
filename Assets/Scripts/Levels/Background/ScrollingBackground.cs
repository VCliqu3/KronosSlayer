using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float speedX;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Vector2 position;
    private float pixelsPerUnitBG;

    public float _sizeX = 1920;
    public float initPosX;
    public float limitX;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        pixelsPerUnitBG = _spriteRenderer.sprite.pixelsPerUnit;
        initPosX = transform.position.x;
        limitX = _sizeX / pixelsPerUnitBG;  
    }

    // Update is called once per frame
    void Update()
    {      
        _rigidbody2D.velocity = new Vector2(speedX, 0);
        RepositionBackground();         
    }

    void RepositionBackground()
    {
        position = transform.position;
       
        if ((position.x >= initPosX + limitX) || (position.x <= initPosX - limitX))
        {
            position.x = initPosX;
            transform.position = position;
        }
        
    }
}
