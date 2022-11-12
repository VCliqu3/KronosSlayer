using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKAttackController : MonoBehaviour
{
    private Animator _animator;
    private KKMovementController _KKMovementController;
    public LayerMask playerLayer;

    public float attackRange = 5f;
    public float maxAttackRange = 5f;

    public float attackDuration = 2;
    public float attackDamage = 3f;
    public float attackShieldPenetration = 0f;

    public Transform attackPoint;

    public bool playerOnAttackRange;
    public bool playerOnMaxAttackRange;

    public float heightAttackArea;
    public float lenghtAttackArea;

    public bool isAttacking;

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

    public void DamagePlayer()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapAreaAll(new Vector2(attackPoint.position.x - lenghtAttackArea / 2, attackPoint.position.y - heightAttackArea / 2), new Vector2(attackPoint.position.x + lenghtAttackArea / 2, attackPoint.position.y + heightAttackArea / 2), playerLayer);

        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<HealthController>().TakeDamage(attackDamage, attackShieldPenetration);
        }
    }
}
