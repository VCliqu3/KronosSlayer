using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [HideInInspector]
    public Animator _animator;

    private MeleeController _meleeController;
    private HealthController _healthController;
    private HandHeadController _handHeadController;

    public float movementSpeed = 3f;
    public float velX;

    public float jumpForce = 3f;
    public float fallMultiplier = 0.5f;
    public float lowJumpMultiplier = 1f;

    public bool isGrounded;
    public Transform feetPos;
    public float rectangleLenght;
    public float rectangleHeight;
    public LayerMask whatIsGround;
    public LayerMask enemies;

    public bool playerFacingRight = true;

    public bool betterJump = true;
    public bool movementEnable = true;
    public bool jumpingEnable = true;
    public bool dashEnabled = true;
    public bool dashThroughProyectiles = false;

    public float dashForce = 25f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;

    public bool isDashing = false;
    public bool dashingBehind = false;
    public float dashCooldownCounter;

    private bool pseudoEnableJumping = true;

    private TrailRenderer _trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _meleeController = GetComponent<MeleeController>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _healthController = GetComponent<HealthController>();
        _handHeadController = GetComponent<HandHeadController>();

        dashCooldownCounter = dashCooldown;
    }

    void Update()
    {
        Dash();
    }
    void FixedUpdate()
    {     
        EnableDisableMovement();
        EnableDisableJumping();

        Move();
        Jump();
    
        RotatePlayer();
    }

    void EnableDisableMovement()
    {
        if(!_meleeController.isAttacking && !_meleeController.isOnAttackTransition && !isDashing && !_healthController.isHurting) 
        {
            movementEnable = true;
        }
        else
        {
            movementEnable = false;
        }
    }

    void EnableDisableJumping()
    {
        if (!_meleeController.isAttacking && !_meleeController.isOnAttackTransition && !isDashing) //&& !_meleeController.isOnAttackTransition
        {
            jumpingEnable = true;
        }
        else
        {
            jumpingEnable = false;
        }
    }
    void Move()
    {
        if (movementEnable)
        {
            velX = Input.GetAxisRaw("Horizontal") * movementSpeed;
            _rigidbody2D.velocity = new Vector2(velX, _rigidbody2D.velocity.y);

        }  
    }

    
    void Jump()
    {
        isGrounded = Physics2D.OverlapArea(new Vector2 (feetPos.position.x - rectangleLenght/2,feetPos.position.y-rectangleHeight/2), new Vector2(feetPos.position.x + rectangleLenght / 2, feetPos.position.y + rectangleHeight / 2), whatIsGround | enemies);

        if (jumpingEnable)
        {         
            if (isGrounded && !Input.GetKey(KeyCode.Space)) //Hailita el salto 
            {
                pseudoEnableJumping = true;
            }

            if (Input.GetKey(KeyCode.Space) && isGrounded && pseudoEnableJumping && !_healthController.isHurting) //Si se presiona espacio, salta
            {
                _rigidbody2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                pseudoEnableJumping = false;
            }

            if (betterJump)
            {
                if (_rigidbody2D.velocity.y < 0)
                {
                    _rigidbody2D.velocity += (fallMultiplier) * Physics2D.gravity.y * Time.fixedDeltaTime * Vector2.up; //Vector2.up = (0,1)
                }

                if ((_rigidbody2D.velocity.y > 0 && !(Input.GetKey(KeyCode.Space)))||_healthController.isHurting)
                {
                    _rigidbody2D.velocity += (lowJumpMultiplier) * Physics2D.gravity.y * Time.fixedDeltaTime * Vector2.up; //Vector2.up = (0,1)
                }
            }
        }
    }  
    void RotatePlayer()
    {
        if (!_meleeController.attackingBehind)
        {
            if (velX > 0 && !playerFacingRight)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                playerFacingRight = !playerFacingRight;
            }
            else if (velX < 0 && playerFacingRight)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                playerFacingRight = !playerFacingRight;
            }
        }
    }

    void Dash()
    {
        if (Input.GetKeyDown("e") && dashEnabled && !_meleeController.isAttacking && !_healthController.isHurting)
        {
            _animator.SetTrigger("Dash");
            StartCoroutine(Dashing());
            StartCoroutine(DashCooldown());
        }
    }
    IEnumerator Dashing()
    {
        dashEnabled = false;
        isDashing = true;

        dashCooldownCounter = 0;

        float originalGravity = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = 0f;

        if (_handHeadController.aimAngle > 90 && _handHeadController.aimAngle < 270)
        {
            _meleeController.ForcedPlayerRotation();
            dashingBehind = true;
        }

        _rigidbody2D.velocity = transform.right * dashForce;

        _trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTime);
        _trailRenderer.emitting = false;

        _rigidbody2D.gravityScale = originalGravity;

        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y); //Importante para arreglar bug de GetHurt mientras se dashea

        isDashing = false;
        dashingBehind = false;
       
    }
    IEnumerator DashCooldown()
    {
        while (dashCooldownCounter < dashCooldown)
        {
            dashCooldownCounter += Time.deltaTime;
            yield return null;
        }

        dashCooldownCounter = dashCooldownCounter > dashCooldown ? dashCooldown : dashCooldownCounter;

        dashEnabled = true;
    }

}
