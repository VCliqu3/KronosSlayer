using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileMovementController : MonoBehaviour
{

    public Rigidbody2D _rigidbody2D;
    public float speed = 1f;
    public Vector2 direction;

    public float secondsToDestroy = 5f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Destroy(gameObject, secondsToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        _rigidbody2D.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
           
            Destroy(gameObject);
            
        }
    }
}
