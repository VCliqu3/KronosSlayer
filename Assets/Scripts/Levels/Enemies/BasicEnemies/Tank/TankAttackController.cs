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
                float startingHealth = player.GetComponent<HealthController>().health;

                if (player.GetComponent<HealthController>().shield > 0)
                {
                    CreateShieldImpacVFX(player.transform, playerSIScale, 0.5f);
                }

                player.GetComponent<HealthController>().TakeDamage(attackDamage, attackShieldPenetration);

                if (player.GetComponent<HealthController>().health < startingHealth)
                {
                    //
                }
            }
        }
    }

    public void CreateShieldImpacVFX(Transform entHit, float scale, float offsetY)
    {
        GameObject ShImpVFX = Instantiate(ShieldImpactVFX, entHit.position + new Vector3(0f, offsetY), entHit.transform.rotation);

        ShImpVFX.transform.localScale = ShImpVFX.transform.localScale * scale;

        AvoidParentRotation _APR = ShImpVFX.GetComponent<AvoidParentRotation>();

        _APR.hitInitialPos = entHit.position + new Vector3(0f, offsetY);
        _APR.entityHitTranform = entHit;
        _APR.CalculateOffsetVector();

        Destroy(ShImpVFX, 1.2f);
    }

}
