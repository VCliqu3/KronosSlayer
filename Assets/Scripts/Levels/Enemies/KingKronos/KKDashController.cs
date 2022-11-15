using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKDashController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private KKMovementController _KKMovementController;
    private KKHealthController _KKHealthController;

    public float dashRangeMin = 5f;
    public float dashRangeMax = 7f;

    public bool playerOnDashRange = false;

    public bool dashEnabled = true;

    public float timeChargingDash = 1f;
    public float enragedTimeChargingDash = 0.8f;

    public float dashForce = 15f;
    public float enragedDashForce = 25f;

    public float dashTime;

    public float dashCooldown = 8f;
    public float enragedDashCooldown = 5f;

    public float dashCooldownCounter;

    public bool isDashing = false;

    public float chargeDamageReduction = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _KKMovementController = GetComponent<KKMovementController>();
        _KKHealthController = GetComponent<KKHealthController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerOnDashRange = (_KKMovementController.DetectPlayer(dashRangeMax, "front") && !_KKMovementController.DetectPlayer(dashRangeMin, "front"));
        //Dash();
    }

    public void Dash()
    {
        if (dashEnabled && playerOnDashRange)
        {
            StartCoroutine(Dashing());
            StartCoroutine(DashCooldown());
        }
    }

    IEnumerator Dashing()
    {    
        dashEnabled = false;

        _KKHealthController.damageReduction = chargeDamageReduction;
        _animator.Play("ChargeDash");

        dashCooldownCounter = 0;

        yield return new WaitForSeconds(timeChargingDash);
        _KKHealthController.damageReduction = 0;

       float playerPosX = FindObjectOfType<MovementController>().transform.position.x;
        float distanceToDash = Mathf.Abs(transform.position.x - playerPosX);

        dashTime = distanceToDash / dashForce;

        isDashing = true;
        _animator.SetTrigger("Dash");

        _rigidbody2D.velocity = transform.right * dashForce;

        yield return new WaitForSeconds(dashTime);

        _KKMovementController.Stop();

        isDashing = false;

        _animator.Play("Attack");

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
