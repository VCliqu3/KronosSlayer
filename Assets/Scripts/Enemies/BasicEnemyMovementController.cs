using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMovementController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    public Animator _animator;

    public float walkSpeed = 1f;
    public float runSpeed = 2f;

    public float timePatrolWalk = 3f;
    public Transform sightPoint;
    public Transform groundDetector;
    public float frontSightDistance;
    public float backSightDistance;
    public float groundDetectionDistance;

    public bool playerOnSight;
    public bool playerOnSightBack;

    public bool groundInFront = false;
    public bool groundDown = true;

    public bool isFacingRight = false;

    public float maxTimeStandingPatrol = 2f;
    public float minTimeStandingPatrol = 1f;

    public bool isPatrolling = false;
    public IEnumerator patrolCoroutine;

    public bool canTurnBack = true;

    // Start is called before the first frame update
    void Start()
    {
        
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        patrolCoroutine = Patrol();
    }

    // Update is called once per frame
    void Update()
    {
        playerOnSight = DetectPlayer(frontSightDistance, "front");
        playerOnSightBack = DetectPlayer(backSightDistance, "back");

        groundInFront = DetectGround(groundDetectionDistance, "front");
        groundDown = DetectGround(groundDetectionDistance, "down");


        EnableDisableCanTurnBack();

        if (playerOnSightBack && canTurnBack)
        {
            ForcedRotation();
        }
        
    }

    public void StartPatrol()
    {
        StartCoroutine(patrolCoroutine);
    }

    public void StopPatrol()
    {
        StopCoroutine(patrolCoroutine);
        isPatrolling = false;
    }

    public IEnumerator Patrol()
    {

        _animator.SetBool("Walk", true);

        float speedAndDirecion = isFacingRight ? walkSpeed : -walkSpeed;
        float timeWalking = 0f;

        while (timeWalking < timePatrolWalk && !groundInFront && groundDown)
        {
            _rigidbody2D.velocity = new Vector2(speedAndDirecion, _rigidbody2D.velocity.y);
            timeWalking += Time.deltaTime;
            yield return null;

        }         
    
        _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y);

        _animator.SetBool("Walk", false);

        float timeStanding = Random.Range(minTimeStandingPatrol, maxTimeStandingPatrol);

        yield return new WaitForSeconds(timeStanding);

        ForcedRotation();

        playerOnSight = DetectPlayer(frontSightDistance, "front"); //Actializamos valores para evitar bugs

        isPatrolling = false;

        groundInFront = DetectGround(groundDetectionDistance, "front"); //Actializamos valores para evitar bugs
        groundDown = DetectGround(groundDetectionDistance, "down");

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

        Debug.DrawRay(sightPoint.position, distance*transform.right, Color.green);

        return detectPlayer;
    }

    public bool DetectGround(float distance, string direction) //Para ver si el jugador ha sido detectado por el RayCast, con una maxima distancia de deteccion de distance
    {
        bool detectGround = false;

        Vector2 endPos;

        if (direction == "front")
        {
            endPos = groundDetector.position + groundDetector.right * distance;
        }
        else if (direction == "down")
        {
            endPos = groundDetector.position - groundDetector.up * distance;
        }
        else
        {
            endPos = groundDetector.position + groundDetector.right * distance;
        }


        RaycastHit2D hit = Physics2D.Linecast(groundDetector.position, endPos, 1 << LayerMask.NameToLayer("Ground"));

        if (hit.collider != null)
        {
            detectGround = true;
        }

        Debug.DrawRay(groundDetector.position, distance * groundDetector.right, Color.blue);
        Debug.DrawRay(groundDetector.position, -distance * groundDetector.up, Color.blue);

        return detectGround;
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

    public void EnableDisableCanTurnBack()
    {
        if(playerOnSight)
        {
            if (groundDown && !groundInFront)
            {
                canTurnBack = true;
            }
            else
            {
                canTurnBack = false;
            }
        }
    }
}
