using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

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
    public GameObject PlayerImpactVFX;
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
            HealthController _healthController = player.GetComponent<HealthController>();

            if (!_healthController.invincibilityEnabled)
            {
                float startingHealth = _healthController.health;

                if (_healthController.shield > 0)
                {
                    CreateFeedbackImpactVFX(ShieldImpactVFX, player.transform, playerSIScale, 0.5f, 1.2f);
                }

                _healthController.TakeDamage(attackDamage, attackShieldPenetration);

                if (_healthController.health < startingHealth)
                {
                    CreateFeedbackImpactVFX(PlayerImpactVFX, player.transform, playerSIScale, 0.5f, 1.2f);
                }
            }
        }

        CameraShaker.Instance.ShakeOnce(1f, 2f, 0.1f, 2f);
    }

    public void CreateFeedbackImpactVFX(GameObject feedbackVFX, Transform entHit, float scale, float offsetY, float timeToAutodestroy)
    {
        GameObject fVFX = Instantiate(feedbackVFX, entHit.position + new Vector3(0f, offsetY), entHit.transform.rotation);

        fVFX.transform.localScale = fVFX.transform.localScale * scale;

        AvoidParentRotation _APR = fVFX.GetComponent<AvoidParentRotation>();

        _APR.hitInitialPos = entHit.position + new Vector3(0f, offsetY);
        _APR.entityHitTranform = entHit;
        _APR.CalculateOffsetVector();

        Destroy(fVFX, timeToAutodestroy);
    }

    public void CreateGroundImpactVFX(GameObject groundImpVFX, Transform point, float scaleX, float offsetY, float timeToAutodestroy)
    {
        GameObject fVFX = Instantiate(groundImpVFX, point.position + new Vector3(0f, offsetY), point.transform.rotation);

        fVFX.transform.localScale = new Vector2(fVFX.transform.localScale.x * scaleX,fVFX.transform.localScale.y);

        Destroy(fVFX, timeToAutodestroy);
    }

}
