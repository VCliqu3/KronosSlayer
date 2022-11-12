using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKDashController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private KKMovementController _KKMovementController;
    private KKAttackController _KKAttackController;

    public float dashRangeMin = 5f;
    public float dashRangeMax = 7f;

    public bool playerOnDashRange = false;


    public bool dashEnabled = true;

    public float dashForce = 25f;
    public float dashTime = 0.2f;

    public float dashDamage = 3f;
    public float dashShieldPenetration = 0f;

    public float dashCooldown = 1f;
    public float dashCooldownCounter;

    public bool isDashing = false;



    public Transform dashAttackPoint;

    // Start is called before the first frame update
    void Start()
    {
        _KKMovementController = GetComponent<KKMovementController>();
        _KKAttackController = GetComponent<KKAttackController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerOnDashRange = (_KKMovementController.DetectPlayer(dashRangeMax, "front") && !_KKMovementController.DetectPlayer(dashRangeMin, "front"));
        //Dash();
    }

    void Dash()
    {
        if (dashEnabled && playerOnDashRange && !_KKAttackController.playerOnAttackRange )
        {
            //_animator.SetTrigger("Dash");
            StartCoroutine(Dashing());
            StartCoroutine(DashCooldown());
        }
    }

    IEnumerator Dashing()
    {
        dashEnabled = false;
        isDashing = true;
        dashCooldownCounter = 0;

        _rigidbody2D.velocity = transform.right * dashForce;

        yield return new WaitForSeconds(dashTime);

        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y); //Importante para arreglar bug de GetHurt mientras se dashea

        isDashing = false;

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
