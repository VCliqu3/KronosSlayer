using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKTPController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private KKMovementController _KKMovementController;
    private KKHealthController _KKHealthController;

    public bool TPEnabled = true;

    public float TPAttackDamage = 3f;
    public float TPAttacShieldPenetration = 0f;

    public float timeChargingTP = 1f;
    public float timeStayingUp = 1f;
    public float timeOnGround = 1f;
    public float distanceToAppearUp = 2f;
    public float downImpulse = 2f;

    public bool isTPAttacking = false;

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
        //TPAttack();
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

        _animator.Play("ChargeTP");

        yield return new WaitForSeconds(timeChargingTP);

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

        _animator.SetTrigger("LandTPAttack");

        yield return new WaitForSeconds(timeOnGround);

        isTPAttacking = false;

        _KKHealthController.CallEmptyDamageAccumulated();

        TPEnabled = true;
        _animator.Play("Idle");

    }
}
