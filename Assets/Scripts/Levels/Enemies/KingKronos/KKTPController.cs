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
    public float enragedtimeChargingTP = 1f;

    public float timeStayingUp = 1f;
    public float enragedTimeStayingUp = 0.1f;
    public float timeOnGround = 1f;

    public float distanceToAppearUp = 2f;

    public float downImpulse = 2f;
    public float enrageddownImpulse = 2f;

    public bool isTPAttacking = false;

    public float chargeDamageReduction = 0.5f;

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

        _animator.SetTrigger("StayUpTPAttack");

        yield return new WaitForSeconds(timeStayingUp);

        _rigidbody2D.gravityScale = originalGravity;

        _animator.SetTrigger("TPAttack");

        _rigidbody2D.AddForce(new Vector2(0,-downImpulse), ForceMode2D.Impulse);

        while (!_KKMovementController.isGrounded)
        {
            yield return null;
        }

        DamageTPAttackPlayer();

        CameraShaker.Instance.ShakeOnce(1f, 2f, 0.1f, 2f);
        _animator.SetTrigger("LandTPAttack");

        yield return new WaitForSeconds(timeOnGround);

        isTPAttacking = false;

        if (_KKHealthController.isEnraged && doubleTPAttackWhenEnraged)
        {
            isTPAttacking = true;

            _animator.Play("ChargeTP");

            yield return new WaitForSeconds(timeChargingTP);

            Vector2 playerPos_ = FindObjectOfType<MovementController>().transform.position;
            transform.position = new Vector2(playerPos_.x, playerPos_.y + distanceToAppearUp);
            _KKMovementController.ForcedRotation();

            float originalGravity_ = _rigidbody2D.gravityScale;
            _rigidbody2D.gravityScale = 0f;

            _animator.SetTrigger("StayUpTPAttack");

            yield return new WaitForSeconds(timeStayingUp);

            _rigidbody2D.gravityScale = originalGravity_;

            _animator.SetTrigger("TPAttack");

            _rigidbody2D.AddForce(new Vector2(0, -downImpulse), ForceMode2D.Impulse);

            while (!_KKMovementController.isGrounded)
            {
                yield return null;
            }

            DamageTPAttackPlayer();

            CameraShaker.Instance.ShakeOnce(1f, 2f, 0.1f, 2f);
            _animator.SetTrigger("LandTPAttack");

            yield return new WaitForSeconds(timeOnGround);

        }

        isTPAttacking = false;

        _KKHealthController.CallEmptyDamageAccumulated();

        TPEnabled = true;
        _animator.Play("Idle");

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
