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

    public GameObject ShieldImpactVFX;
    public GameObject PlayerImpactVFX;

    public float playerSIScale = 1f;

    public GameObject KingKronosGroundImpactVFX;

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

        CameraShaker.Instance.ShakeOnce(1f, 1f, 0.1f, 1f);
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

        fVFX.transform.localScale = new Vector2(fVFX.transform.localScale.x * scaleX, fVFX.transform.localScale.y);

        Destroy(fVFX, timeToAutodestroy);
    }
}
