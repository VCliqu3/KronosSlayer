using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKJumpAttackController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private KKMovementController _KKMovementController;
    private KKDashController _KKDashController;

    public float jumpRangeMin = 7f;
    public float jumpRangeMax = 9f;

    public bool playerOnJumpRange = false;

    public bool jumpEnabled = true;

    public float jumpAngle = 45;
    public float fallSpeed = 5f;
    public float timeStayingUp = 0.5f;
    public float timeChargingJump = 1f;
    public float timeOnGround = 1f;

    public float jumpAttackDamage = 3f;
    public float jumpAttackShieldPenetration = 0f;

    public float jumpCooldown = 5f;
    public float jumpCooldownCounter;

    public bool isJumpAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        _KKMovementController = GetComponent<KKMovementController>();
        _KKDashController = GetComponent<KKDashController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerOnJumpRange = (_KKMovementController.DetectPlayer(jumpRangeMax, "front") && !_KKMovementController.DetectPlayer(jumpRangeMin, "front"));
        //Jump();
    }
    public void Jump()
    {
        if (jumpEnabled && playerOnJumpRange)
        {
            StartCoroutine(Jumping());        
            StartCoroutine(JumpCooldown());
        }
    }

    IEnumerator Jumping()
    {
        isJumpAttacking = true;
        jumpEnabled = false;

        _animator.Play("ChargeJump");

        jumpCooldownCounter = 0f;

        yield return new WaitForSeconds(timeChargingJump);

        float playerPosX = FindObjectOfType<MovementController>().transform.position.x;
        float playerPosY = FindObjectOfType<MovementController>().transform.position.y;
        float distanceToJump = Mathf.Abs(transform.position.x - playerPosX);
        float jumpAngleToRadians = jumpAngle * Mathf.PI / 180;
        float jumpForce = Mathf.Sqrt(distanceToJump * 9.81f / Mathf.Sin(2 * jumpAngleToRadians));

        /*
        if (_KKMovementController.isFacingRight) //Ambas formas funcionan
        {
            _rigidbody2D.velocity = new Vector2(jumpForce * Mathf.Cos(jumpAngleToRadians), jumpForce * Mathf.Sin(jumpAngleToRadians));
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(-jumpForce * Mathf.Cos(jumpAngleToRadians), jumpForce * Mathf.Sin(jumpAngleToRadians));
        }
        */

        _animator.SetTrigger("Jump");

        if (_KKMovementController.isFacingRight)
        {
            _rigidbody2D.AddForce(new Vector2(jumpForce * Mathf.Cos(jumpAngleToRadians), jumpForce * Mathf.Sin(jumpAngleToRadians)), ForceMode2D.Impulse);
        }
        else
        {
            _rigidbody2D.AddForce(new Vector2(-jumpForce * Mathf.Cos(jumpAngleToRadians), jumpForce * Mathf.Sin(jumpAngleToRadians)), ForceMode2D.Impulse);
        }

        while(_rigidbody2D.velocity.y > 0)
        {
            yield return null;
        }

        float originalGravity = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = 0f;

        _KKMovementController.Stop();
        _KKMovementController.StopOnY();

        yield return new WaitForSeconds(timeStayingUp);

        Vector2 fallDirection = new Vector2(playerPosX - transform.position.x, playerPosY - transform.position.y).normalized;

        _animator.SetTrigger("JumpAttack");

        _rigidbody2D.velocity = fallDirection * fallSpeed;

        while (!_KKMovementController.isGrounded)
        {
            yield return null;
        }

        _rigidbody2D.gravityScale = originalGravity;

        _KKMovementController.Stop();

        _animator.SetTrigger("LandJumpAttack");

        yield return new WaitForSeconds(timeOnGround);

        isJumpAttacking = false;

        _animator.Play("Idle");
    }
    IEnumerator JumpCooldown()
    {
        while (jumpCooldownCounter < jumpCooldown)
        {
            jumpCooldownCounter += Time.deltaTime;

            yield return null;
        }

        jumpCooldownCounter = jumpCooldownCounter > jumpCooldown ? jumpCooldown : jumpCooldownCounter;

        jumpEnabled = true;
    }
}
