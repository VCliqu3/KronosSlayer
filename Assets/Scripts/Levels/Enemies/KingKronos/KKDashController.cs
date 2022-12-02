using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class KKDashController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private KKMovementController _KKMovementController;
    private KKHealthController _KKHealthController;
    private KKAttackController _KKAttackController;

    public LayerMask playerLayer;
    public Transform dashAttackPoint;
    public float attackRadius = 1f;

    public float dashRangeMin = 5f;
    public float dashRangeMax = 7f;

    public bool playerOnDashRange = false;

    public bool dashEnabled = true;

    public float dashAttackDamage = 7f;
    public float enragedDashAttackDamage = 9f;

    public float dashAttackShieldPenetration = 0f;
    public float enragedDashAttackShieldPenetration = 0f;

    public float timeChargingDash = 1f;
    public float enragedTimeChargingDash = 0.8f;

    public float timeOnGround = 1f;

    public float dashForce = 15f;
    public float enragedDashForce = 25f;

    public float dashTime;

    public float dashCooldown = 8f;
    public float enragedDashCooldown = 5f;

    public float dashCooldownCounter;

    public bool isDashing = false;

    public float chargeDamageReduction = 0.5f;

    public TrailRenderer _trailRenderer;

    private DashShadowsController _dashShadowsController;
    public GameObject abilityShadow;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _KKMovementController = GetComponent<KKMovementController>();
        _KKHealthController = GetComponent<KKHealthController>();
        _dashShadowsController = GetComponent<DashShadowsController>();
        _KKAttackController = GetComponent<KKAttackController>();
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

        _dashShadowsController.shadow = abilityShadow;
        _dashShadowsController.enableShadows = true;
        //_trailRenderer.emitting = true;

        yield return new WaitForSeconds(dashTime);

        _dashShadowsController.enableShadows = false;
        //_trailRenderer.emitting = false;

        _KKMovementController.Stop();

        _animator.SetTrigger("DashAttack");

        yield return new WaitForSeconds(timeOnGround);

        _animator.SetTrigger("GetUp");
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

    public void DamageDashAttackPlayer()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(dashAttackPoint.position, attackRadius, playerLayer);

        foreach (Collider2D player in hitPlayer)
        {
            if (!player.GetComponent<HealthController>().invincibilityEnabled)
            {
                float startingHealth = player.GetComponent<HealthController>().health;

                if (player.GetComponent<HealthController>().shield > 0)
                {
                    _KKAttackController.CreateFeedbackImpactVFX(_KKAttackController.ShieldImpactVFX,player.transform, _KKAttackController.playerSIScale, 0.5f,1.2f);
                }

                player.GetComponent<HealthController>().TakeDamage(dashAttackDamage, dashAttackShieldPenetration);

                if (player.GetComponent<HealthController>().health < startingHealth)
                {
                    //
                }
            }
        }

        CameraShaker.Instance.ShakeOnce(1f, 1f, 0.1f, 1f);
    }

    public void SetIsDashingFalse()
    {
        isDashing = false;
    }
}
