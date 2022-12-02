using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttackController : MonoBehaviour
{
    private Animator _animator;
    private BasicEnemyMovementController _basicEnemyMovementController;
    public LayerMask playerLayer;

    public float timeRemainingFollowing = 1.5f;

    public float attackRange = 5f;
    public float maxAttackRange = 5f;
    public float attackRangeBack = 1f;
    public float attackDuration = 2;
    public float attackDamage = 3f;
    public float attackShieldPenetration = 0f;

    public Transform attackPoint;
    public bool playerOnAttackRange;
    public bool playerOnMaxAttackRange;
    public bool playerOnAttackRangeBack;

    public float heightAttackArea;
    public float lenghtAttackArea;

    public bool isAttacking;

    public GameObject ShieldImpactVFX;
    public float playerSIScale = 1f;

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
        playerOnAttackRangeBack = _basicEnemyMovementController.DetectPlayer(attackRangeBack, "back");

    }

    public void DamagePlayer()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapAreaAll(new Vector2(attackPoint.position.x - lenghtAttackArea / 2, attackPoint.position.y - heightAttackArea / 2), new Vector2(attackPoint.position.x + lenghtAttackArea / 2, attackPoint.position.y + heightAttackArea / 2),playerLayer);

        foreach (Collider2D player in hitPlayer)
        {
            if (!player.GetComponent<HealthController>().invincibilityEnabled)
            {
                if (player.GetComponent<HealthController>().shield > 0)
                {
                    GameObject ShImpVFX = Instantiate(ShieldImpactVFX, player.transform.position + new Vector3(0f, 0.5f), transform.rotation);

                    ShImpVFX.transform.localScale = ShImpVFX.transform.localScale * playerSIScale;

                    AvoidParentRotation _APR = ShImpVFX.GetComponent<AvoidParentRotation>();

                    _APR.hitInitialPos = player.transform.position + new Vector3(0f, 0.5f);
                    _APR.entityHitTranform = player.transform;
                    _APR.CalculateOffsetVector();

                    Destroy(ShImpVFX, 1.2f);
                }

                player.GetComponent<HealthController>().TakeDamage(attackDamage, attackShieldPenetration);
            }
        }
    }

}
