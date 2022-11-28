using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class KKMovementController : MonoBehaviour
{
    public bool isActivated = false;

    private Rigidbody2D _rigidbody2D;
    private KKHUDController _KKHUDController;
    private KKHealthController _KKHealthController;
    private KKAttackController _KKAttackController;
    private KKJumpAttackController _KKJumpAttackController;
    private KKDashController _KKDashController;
    private KKTPController _KKTPController;
    [HideInInspector]
    public Animator _animator;

    public float runSpeed = 2.5f;
    public float enragedRunSpeed = 3.5f;
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

    public Animator KKCanvasAnimator;
    public float timeStayingUpOpening;
    public float timeOnGroundOpening;
    public float fallImpulseOpening;

    public TrailRenderer _trailRenderer;

    private DashShadowsController _dashShadowsController;
    public GameObject abilityShadow;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 11);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _KKHUDController = FindObjectOfType<KKHUDController>();
        _KKHealthController = GetComponent<KKHealthController>();
        _KKAttackController = GetComponent<KKAttackController>();
        _KKJumpAttackController = GetComponent<KKJumpAttackController>();
        _KKDashController = GetComponent<KKDashController>();
        _KKTPController = GetComponent<KKTPController>();
        _dashShadowsController = GetComponent<DashShadowsController>();

        StartCoroutine(OpeningScene());
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGround();
        playerOnSight = DetectPlayer(frontSightDistance, "front");
        playerOnSightBack = DetectPlayer(backSightDistance, "back");

        if (playerOnSightBack && !playerOnSight && canTurnBack && !_KKHealthController.isDead)
        {
            ForcedRotation();
        }

        EnableDisableCanTurnBack();
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

    public void EnableDisableCanTurnBack()
    {  
        if (!_KKJumpAttackController.isJumpAttacking && !_KKDashController.isDashing && !_KKTPController.isTPAttacking && !_KKAttackController.isAttacking && isActivated &&!_KKHealthController.onEnrageAnim)
        {
            canTurnBack = true;
        }
        else
        {
            canTurnBack = false;
        }
        
    }

    public IEnumerator OpeningScene()
    {
        KKCanvasAnimator.SetTrigger("FadeIn");

        _KKHUDController.SetHealthBar();
        _KKHUDController.SetShieldBar();
        _KKHUDController.SetDamageAccumulationBar();

        float originalGravity = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = 0f;

        _animator.Play("StayUpOpening");

        yield return new WaitForSeconds(timeStayingUpOpening);

        _rigidbody2D.gravityScale = originalGravity;

        _animator.SetTrigger("FallOpening");
        _rigidbody2D.AddForce(new Vector2(0, -fallImpulseOpening), ForceMode2D.Impulse);

        _dashShadowsController.shadow = abilityShadow;
        _dashShadowsController.enableShadows = true;
        //_trailRenderer.emitting = true;

        while (!isGrounded)
        {
            yield return null;
        }

        _dashShadowsController.enableShadows = false;
        //_trailRenderer.emitting = false;

        CameraShaker.Instance.ShakeOnce(1f, 2f, 0.1f, 2f);
        _animator.SetTrigger("Land");

        yield return new WaitForSeconds(timeOnGroundOpening);

        _animator.SetTrigger("OPAnim");
    }
}
