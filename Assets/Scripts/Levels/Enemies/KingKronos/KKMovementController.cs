using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKMovementController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [HideInInspector]
    public Animator _animator;

    public float walkSpeed = 1f;
    public float runSpeed = 2f;
    public float timeRemainingFollowing = 1.5f;

    public Transform sightPoint;

    public float frontSightDistance;
    public float backSightDistance;

    public bool playerOnSight;
    public bool playerOnSightBack;

    public bool isFacingRight = false;

    public bool canTurnBack = true;

    public LayerMask whatIsGround;
    public bool isGrounded;
    public float rectangleLenght;
    public float rectangleHeight;

    //private BasicEnemyHealthController _basicEnemyHealthController;


    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 11);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGround();
        playerOnSight = DetectPlayer(frontSightDistance, "front");
        playerOnSightBack = DetectPlayer(backSightDistance, "back");

        if (playerOnSightBack && !playerOnSight && canTurnBack) //&& !_basicEnemyHealthController.isDead
        {
            ForcedRotation();
        }
    }

    public bool DetectPlayer(float distance, string direction) //Para ver si el jugador ha sido detectado por el RayCast, con una maxima distancia de deteccion de distance
    {
        bool detectPlayer = false;

        Vector2 endPos;

        if (direction == "front")
        {
            endPos = sightPoint.position + sightPoint.right * distance;
        }
        else if (direction == "back")
        {
            endPos = sightPoint.position - sightPoint.right * distance;
        }
        else
        {
            endPos = sightPoint.position + sightPoint.right * distance;
        }

        RaycastHit2D hit = Physics2D.Linecast(sightPoint.position, endPos, 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ground"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                detectPlayer = true;
            }
            else
            {
                detectPlayer = false;
            }
        }

        if (direction == "front")
        {
            Debug.DrawRay(sightPoint.position, distance * transform.right, Color.green);
        }
        else if (direction == "back")
        {
            Debug.DrawRay(sightPoint.position, distance * -transform.right, Color.green);

        }
        
        return detectPlayer;
    }

    public void Run()
    {
        float speedAndDirecion = isFacingRight ? runSpeed : -runSpeed;

        _rigidbody2D.velocity = new Vector2(speedAndDirecion, _rigidbody2D.velocity.y);
    }

    public void Stop()
    {
        _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y);
    }

    public void StopOnY()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
    }

    public void ForcedRotation()
    {
        if (!isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            isFacingRight = !isFacingRight;

        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            isFacingRight = !isFacingRight;
        }
    }

    public bool CheckGround()
    {
        bool grounded;

        grounded = Physics2D.OverlapArea(new Vector2(transform.position.x - rectangleLenght / 2, transform.position.y - rectangleHeight / 2), new Vector2(transform.position.x + rectangleLenght / 2, transform.position.y + rectangleHeight / 2), whatIsGround);

        return grounded;
    }
}
