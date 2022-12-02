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

    public float creepSIScale = 1f;
    public float tankIScale = 1f;
    public float sniperSIScale = 1f;
    public float KKSIScale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _movementController = GetComponent<MovementController>();
        _healthController = GetComponent<HealthController>();
        _handHeadController = GetComponent<HandHeadController>();
    }

    // Update is called once per frame
    void Update()
    {    
        Attack();
        EnableDisableAttack();
    }

    void EnableDisableAttack()
    {
        if(!ModeController.isRanged && !isAttacking && !_movementController.isDashing && !_healthController.isHurting && !_healthController.playerHasDied && !PauseController.gamePaused && !PauseButonController.mouseOnPauseButton) //&& _movementController.isGrounded si no se quiere que ataque saltando //&& !PauseButonController.mouseOnPauseButton
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
            if (basicEnemy.GetComponent<BasicEnemyHealthController>().shield > 0)
            {
                GameObject ShImpVFX = Instantiate(ShieldImpactVFX, basicEnemy.transform.position, transform.rotation);

                float scaleFactor = 1f;

                if(basicEnemy.GetComponent<CreepShootController>() != null)
                {
                    scaleFactor = creepSIScale;
                }

                if (basicEnemy.GetComponent<TankAttackController>() != null)
                {
                    scaleFactor = tankIScale;
                }

                if (basicEnemy.GetComponent<SniperShootController>() != null)
                {
                    scaleFactor = sniperSIScale;
                }

                ShImpVFX.transform.localScale = ShImpVFX.transform.localScale * scaleFactor;

                AvoidParentRotation _APR = ShImpVFX.GetComponent<AvoidParentRotation>();

                _APR.hitInitialPos = basicEnemy.transform.position;
                _APR.entityHitTranform = basicEnemy.transform;
                _APR.CalculateOffsetVector();

                Destroy(ShImpVFX, 1.2f);
            }

            basicEnemy.GetComponent<BasicEnemyHealthController>().TakeDamage(damage, shieldPenetration);
        }

        Collider2D[] hitKingKronos = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, KingKronosLayer);

        foreach (Collider2D kk in hitKingKronos)
        {
            if (kk.GetComponent<KKHealthController>().canTakeDamage)
            {
                if (kk.GetComponent<KKHealthController>().shield > 0)
                {
                    GameObject ShImpVFX = Instantiate(ShieldImpactVFX, kk.transform.position + new Vector3(0f, 1f), transform.rotation);

                    ShImpVFX.transform.localScale = ShImpVFX.transform.localScale * KKSIScale;

                    AvoidParentRotation _APR = ShImpVFX.GetComponent<AvoidParentRotation>();

                    _APR.hitInitialPos = kk.transform.position + new Vector3(0f, 1f);
                    _APR.entityHitTranform = kk.transform;
                    _APR.CalculateOffsetVector();

                    Destroy(ShImpVFX, 1.2f);
                }

                kk.GetComponent<KKHealthController>().TakeDamage(damage, shieldPenetration);
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

}
