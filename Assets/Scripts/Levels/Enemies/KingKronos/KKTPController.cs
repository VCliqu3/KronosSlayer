using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class KKTPController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private KKMovementController _KKMovementController;
    private KKHealthController _KKHealthController;

    public LayerMask playerLayer;
    public Transform TPAttackPoint;
    public float attackRadius = 1f;

    public bool TPEnabled = true;
    public bool doubleTPAttackWhenEnraged = true;

    public float TPAttackDamage = 7f;
    public float enragedTPAttackDamage = 9f;

    public float TPAttacShieldPenetration = 0f;
    public float enragedTPAttacShieldPenetration = 0f;

    public float timeChargingTP = 1f;
    public float timeChargingEnragedJump = 0.5f;
    public float enragedtimeChargingTP = 1f;

    public float timeStayingUp = 1f;
    public float timeStayingUpEnragedJump = 1f;
    public float enragedTimeStayingUp = 0.1f;
    public float timeOnGround = 1f;

    public float distanceToAppearUp = 2f;
    public float enragedJumpImpulse = 6f;

    public float downImpulse = 2f;
    public float enragedDownImpulse = 2f;

    public bool isTPAttacking = false;

    public float chargeDamageReduction = 0.5f;

    public TrailRenderer _trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _KKMovementController = GetComponent<KKMovementController>();
        _KKHealthController = GetComponent<KKHealthController>();
    }

    public void TPAttack()
    {
        if (TPEnabled && _KKHealthController.damageAccumulatedCounter >= _KKHealthController.damageAccumulationLimit) //&&playerOnTPRange
        {
            StartCoroutine(TPAttacking());
        }
    }

    public IEnumerator TPAttacking()
    {
        TPEnabled = false;
        isTPAttacking = true;
        _KKHealthController.canAccumulateDamage = false;

        _KKHealthController.damageReduction = chargeDamageReduction;
        _animator.Play("ChargeTP");

        yield return new WaitForSeconds(timeChargingTP);
        _KKHealthController.damageReduction = 0;

        Vector2 playerPos = FindObjectOfType<MovementController>().transform.position;
        transform.position = new Vector2(playerPos.x, playerPos.y + distanceToAppearUp);
        _KKMovementController.ForcedRotation();

        float originalGravity = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = 0f;

        _animator.SetTrigger("StayUp");

        yield return new WaitForSeconds(timeStayingUp);

        _rigidbody2D.gravityScale = originalGravity;

        _animator.SetTrigger("TPAttack");

        _rigidbody2D.AddForce(new Vector2(0,-downImpulse), ForceMode2D.Impulse);

        _trailRenderer.emitting = true;

        while (!_KKMovementController.isGrounded)
        {
            yield return null;
        }

        _trailRenderer.emitting = false;

        DamageTPAttackPlayer();

        CameraShaker.Instance.ShakeOnce(1f, 2f, 0.1f, 2f);
        _animator.SetTrigger("Land");

        yield return new WaitForSeconds(timeOnGround);

        if (_KKHealthController.isEnraged && doubleTPAttackWhenEnraged)
        {
            _animator.SetTrigger("Charge");

            yield return new WaitForSeconds(timeChargingEnragedJump);

            _rigidbody2D.AddForce(new Vector2(0, enragedJumpImpulse), ForceMode2D.Impulse);

            _animator.SetTrigger("Jump");

            while (_rigidbody2D.velocity.y >= 0)
            {
                yield return null;
            }

            Vector2 playerPos_ = FindObjectOfType<MovementController>().transform.position;
            transform.position = new Vector2(playerPos_.x, playerPos_.y + distanceToAppearUp);
            _KKMovementController.ForcedRotation();

            _rigidbody2D.gravityScale = 0;

            _animator.SetTrigger("StayUp");

            yield return new WaitForSeconds(timeStayingUpEnragedJump);

            _rigidbody2D.gravityScale = originalGravity;

            _animator.SetTrigger("TPAttack");

            _rigidbody2D.AddForce(new Vector2(0, -downImpulse), ForceMode2D.Impulse);

            _trailRenderer.emitting = true;

            while (!_KKMovementController.isGrounded)
            {
                yield return null;
            }

            _trailRenderer.emitting = false;

            DamageTPAttackPlayer();

            CameraShaker.Instance.ShakeOnce(1f, 2f, 0.1f, 2f);
            _animator.SetTrigger("Land");

            yield return new WaitForSeconds(timeOnGround);
        }

        _animator.SetTrigger("GetUp");

        isTPAttacking = false;
        TPEnabled = true;

        _KKHealthController.CallEmptyDamageAccumulated();
    }

    public void DamageTPAttackPlayer()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(TPAttackPoint.position, attackRadius, playerLayer);

        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<HealthController>().TakeDamage(TPAttackDamage, TPAttacShieldPenetration);
        }
    }
}
