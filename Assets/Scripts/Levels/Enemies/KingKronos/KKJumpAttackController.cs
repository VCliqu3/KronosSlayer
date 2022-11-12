using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKJumpAttackController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private KKMovementController _KKMovementController;
    private KKAttackController _KKAttackController;
    private KKDashController _KKDashController;

    public float jumpRangeMin = 7f;
    public float jumpRangeMax = 9f;

    public bool playerOnJumpRange = false;

    public bool jumpEnabled = true;

    public float jumpAngle = 75;

    public float jumpAttackDamage = 3f;
    public float jumpAttackShieldPenetration = 0f;

    public float jumpCooldown = 5f;
    public float jumpCooldownCounter;

    public bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        _KKMovementController = GetComponent<KKMovementController>();
        _KKAttackController = GetComponent<KKAttackController>();
        _KKDashController = GetComponent<KKDashController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerOnJumpRange = (_KKMovementController.DetectPlayer(jumpRangeMax, "front") && !_KKMovementController.DetectPlayer(jumpRangeMin, "front"));
        Jump();
    }
    void Jump()
    {
        if (jumpEnabled && playerOnJumpRange && !_KKDashController.playerOnDashRange && !_KKAttackController.playerOnAttackRange)
        {
            jumpEnabled = false;
            isJumping = true;
            jumpCooldownCounter = 0f;

            float playerPosX = FindObjectOfType<MovementController>().transform.position.x;
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
      
            if (_KKMovementController.isFacingRight)
            {
                _rigidbody2D.AddForce(new Vector2(jumpForce* Mathf.Cos(jumpAngleToRadians), jumpForce * Mathf.Sin(jumpAngleToRadians)), ForceMode2D.Impulse);
            }
            else
            {
                _rigidbody2D.AddForce(new Vector2(-jumpForce * Mathf.Cos(jumpAngleToRadians), jumpForce * Mathf.Sin(jumpAngleToRadians)), ForceMode2D.Impulse);
            }
           
            StartCoroutine(JumpCooldown());
        }
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
