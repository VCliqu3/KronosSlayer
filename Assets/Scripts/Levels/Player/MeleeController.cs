using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    private MovementController _movementController;
    private HealthController _healthController;
    private HandHeadController _handHeadController;

    public Transform attackPoint;
    public LayerMask BasicEnemyLayers;
    public LayerMask KingKronosLayer;

    public bool stopOnYOnAttack = true;
    public bool attackImpulseOnAir = true;
    public bool cancelGravityOnAttack = true;

    public float attack1Damage;
    public float attack1ShieldPenetration;
    public float attack1Range;
    public float attack1Impulse;

    public float attack2Damage;
    public float attack2ShieldPenetration;
    public float attack2Range;
    public float attack2Impulse;

    public float attack3Damage;
    public float attack3ShieldPenetration;
    public float attack3Range;
    public float attack3Impulse;

    public bool isAttacking = false;
    public bool isOnAttackTransition = false;
    public bool attackingBehind = false;

    private Rigidbody2D _rigidbody2D;

    public bool attackEnable;

    public GameObject ShieldImpactVFX;
    public GameObject BasicEnemyHealthImpactVFX;
    public GameObject KKHealthImpactVFX;

    public float creepSIScale = 1f;
    public float tankIScale = 1f;
    public float sniperSIScale = 1f;
    public float KKSIScale = 1f;

    private LevelController _levelController;

    //SFX

    public string nameSFXenemyShieldImpactSword;

    public string nameSFXcreepTakeDamageSword;
    public string nameSFXtankTakeDamageSword;
    public string nameSFXsniperTakeDamageSword;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _movementController = GetComponent<MovementController>();
        _healthController = GetComponent<HealthController>();
        _handHeadController = GetComponent<HandHeadController>();

        _levelController = FindObjectOfType<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {    
        Attack();
        EnableDisableAttack();
    }

    void EnableDisableAttack()
    {
        if(!ModeController.isRanged && !isAttacking && !_movementController.isDashing && !_healthController.isHurting && !_healthController.playerHasDied && !PauseController.gamePaused && !PauseButonController.mouseOnPauseButton && !_levelController.levelCompleted) //&& _movementController.isGrounded si no se quiere que ataque saltando //&& !PauseButonController.mouseOnPauseButton
        {
            attackEnable = true;
        }
        else
        {
            attackEnable = false;
        }
    }

    public void Attack() 
    {
        if (Input.GetMouseButtonDown(0) && attackEnable)
        {
                  
            if(_handHeadController.aimAngle>90 && _handHeadController.aimAngle < 270)
            {
                ForcedPlayerRotation();
                attackingBehind=!attackingBehind;
            }

            isAttacking = true;

            if (_movementController.isGrounded) //Para frenar cuando se ataca, isGrounded si es que no se quiere frenar si se ataca en el aire;
            {
                _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            }
        }
    }

    public void ForcedPlayerRotation()
    {
        if (!_movementController.playerFacingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _movementController.playerFacingRight = !_movementController.playerFacingRight;

        }
        else if (_movementController.playerFacingRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _movementController.playerFacingRight = !_movementController.playerFacingRight;
        }


    }

    public void DamageEnemies(float damage, float shieldPenetration, float attackRange)
    {
        Collider2D[] hitBasicEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, BasicEnemyLayers);
    
        foreach(Collider2D basicEnemy in hitBasicEnemies)
        {
            BasicEnemyHealthController _BEHealthController = basicEnemy.GetComponent<BasicEnemyHealthController>();
            float startingHealth = _BEHealthController.health;

            if (_BEHealthController.shield > 0)
            {
                if (basicEnemy.GetComponent<CreepShootController>() != null)
                {
                    CreateFeedbackImpactVFX(ShieldImpactVFX, basicEnemy.transform, creepSIScale, 0f, 1.2f);
                }

                if (basicEnemy.GetComponent<TankAttackController>() != null)
                {
                    CreateFeedbackImpactVFX(ShieldImpactVFX, basicEnemy.transform, tankIScale, 0f, 1.2f);
                }

                if (basicEnemy.GetComponent<SniperShootController>() != null)
                {
                    CreateFeedbackImpactVFX(ShieldImpactVFX, basicEnemy.transform, sniperSIScale, 0f, 1.2f);
                }

                AudioManager.instance.PlaySFX(nameSFXenemyShieldImpactSword);
            }

            _BEHealthController.TakeDamage(damage, shieldPenetration);

            if (_BEHealthController.health < startingHealth)
            {
                if (basicEnemy.GetComponent<CreepShootController>() != null)
                {
                    CreateFeedbackImpactVFX(BasicEnemyHealthImpactVFX, basicEnemy.transform, creepSIScale, 0f, 1.2f);
                    AudioManager.instance.PlaySFX(nameSFXcreepTakeDamageSword);
                }

                if (basicEnemy.GetComponent<TankAttackController>() != null)
                {
                    CreateFeedbackImpactVFX(BasicEnemyHealthImpactVFX, basicEnemy.transform, tankIScale, 0f, 1.2f);
                    AudioManager.instance.PlaySFX(nameSFXtankTakeDamageSword);
                }

                if (basicEnemy.GetComponent<SniperShootController>() != null)
                {
                    CreateFeedbackImpactVFX(BasicEnemyHealthImpactVFX, basicEnemy.transform, sniperSIScale, 0f, 1.2f);
                    AudioManager.instance.PlaySFX(nameSFXsniperTakeDamageSword);
                }
            }
        }

        Collider2D[] hitKingKronos = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, KingKronosLayer);

        foreach (Collider2D kk in hitKingKronos)
        {
            KKHealthController _KKHealthController = kk.GetComponent<KKHealthController>();
            float startingHealth = _KKHealthController.health;

            if (_KKHealthController.canTakeDamage)
            {
                if (_KKHealthController.shield > 0)
                {
                    CreateFeedbackImpactVFX(ShieldImpactVFX, kk.transform, KKSIScale, 1f, 1.2f);
                    AudioManager.instance.PlaySFX(nameSFXenemyShieldImpactSword);
                }

               _KKHealthController.TakeDamage(damage, shieldPenetration);
            }

            if (_KKHealthController.health < startingHealth)
            {
                CreateFeedbackImpactVFX(KKHealthImpactVFX, kk.transform, KKSIScale, 1f, 1.2f);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attack1Range);
    }

    public void ImpulsePlayer(float impulseForce)
    {
        if (_movementController.playerFacingRight)
        {
            _rigidbody2D.AddForce(new Vector2 (impulseForce,0f), ForceMode2D.Impulse);
        }
        else
        {
            _rigidbody2D.AddForce(new Vector2(-impulseForce, 0f), ForceMode2D.Impulse);
        }
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
}
