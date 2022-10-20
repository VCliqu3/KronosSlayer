using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttackController : MonoBehaviour
{
    public Animator _animator;
    public BasicEnemyMovementController _basicEnemyMovementController;
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
        _basicEnemyMovementController = GetComponent<BasicEnemyMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerOnAttackRange = _basicEnemyMovementController.DetectPlayer(attackRange, "front");
        playerOnMaxAttackRange = _basicEnemyMovementController.DetectPlayer(maxAttackRange, "front");
    }

    public void DamagePlayer()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapAreaAll(new Vector2(attackPoint.position.x - lenghtAttackArea / 2, attackPoint.position.y - heightAttackArea / 2), new Vector2(attackPoint.position.x + lenghtAttackArea / 2, attackPoint.position.y + heightAttackArea / 2),playerLayer);

        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<HealthController>().TakeDamage(attackDamage, attackShieldPenetration);
        }
    }

}
