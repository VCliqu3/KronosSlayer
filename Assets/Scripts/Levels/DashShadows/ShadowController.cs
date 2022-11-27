using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Shader _material;
    public Color _color;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = Shader.Find("GUI/Text Shader");
    }

    // Update is called once per frame
    void Update()
    {
        ColorSprite();
    }

    void ColorSprite()
    {
        _spriteRenderer.material.shader = _material;
        _spriteRenderer.color = _color;
    }

    public void Finish()
    {
        gameObject.SetActive(false);
    }
}
