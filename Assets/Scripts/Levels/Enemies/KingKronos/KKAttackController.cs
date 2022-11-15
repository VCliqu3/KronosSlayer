using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class KKAttackController : MonoBehaviour
{
    private Animator _animator;
    private KKMovementController _KKMovementController;
    public LayerMask playerLayer;

    public Transform attackPoint;
    public float attackRadius;

    public float attackRange = 5f;
    public float maxAttackRange = 5f;

    public float attackDuration = 2;
    public float enragedAttackDuration = 1.5f;

    public float attackDamage = 5f;
    public float enragedAttackDamage = 8f;

    public float attackShieldPenetration = 0f;
    public float enragedAttackShieldPenetration = 0f;

    public bool playerOnAttackRange;
    public bool playerOnMaxAttackRange;

    public bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _KKMovementController = GetComponent<KKMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerOnAttackRange = _KKMovementController.DetectPlayer(attackRange, "front");
        playerOnMaxAttackRange = _KKMovementController.DetectPlayer(maxAttackRange, "front");
    }

    public void DamageAttackPlayer()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);

        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<HealthController>().TakeDamage(attackDamage, attackShieldPenetration);
        }

        CameraShaker.Instance.ShakeOnce(1f, 1f, 0.1f, 1f);
    }
}
