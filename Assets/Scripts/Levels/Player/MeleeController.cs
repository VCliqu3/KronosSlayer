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

    public float attack1Damage;
    public float attack1ShieldPenetration;
    public float attack1Range;

    public float attack2Damage;
    public float attack2ShieldPenetration;
    public float attack2Range;

    public float attack3Damage;
    public float attack3ShieldPenetration;
    public float attack3Range;

    public bool isAttacking = false;
    public bool isOnAttackTransition = false;
    public bool attackingBehind = false;

    private Rigidbody2D _rigidbody2D;

    public bool attackEnable;

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
        if(!ModeController.isRanged && !isAttacking && !_movementController.isDashing && !_healthController.isHurting && !_healthController.playerHasDied) //&& _movementController.isGrounded si no se quiere que ataque saltando 
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
            basicEnemy.GetComponent<BasicEnemyHealthController>().TakeDamage(damage, shieldPenetration);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attack1Range);
    }

}
