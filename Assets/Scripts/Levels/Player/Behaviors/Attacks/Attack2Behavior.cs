using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2Behavior : StateMachineBehaviour
{
    private MovementController _movementController;
    private MeleeController _meleeController;

    private float originalGravity;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _movementController = animator.gameObject.GetComponent<MovementController>();
        _meleeController = animator.gameObject.GetComponent<MeleeController>();
        _meleeController.DamageEnemies(_meleeController.attack2Damage, _meleeController.attack2ShieldPenetration, _meleeController.attack2Range);

        if (_meleeController.stopOnYOnAttack)
        {
            _movementController.StopOnY();
        }

        if (_movementController.isGrounded || _meleeController.attackImpulseOnAir)
        {
            _movementController.Stop();
            _meleeController.ImpulsePlayer(_meleeController.attack2Impulse);
        }

        if (_meleeController.cancelGravityOnAttack)
        {
            originalGravity = _movementController._rigidbody2D.gravityScale;
            _movementController._rigidbody2D.gravityScale = 0f;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {      
        _meleeController.isAttacking = false;

        if (_meleeController.cancelGravityOnAttack)
        {
            _movementController._rigidbody2D.gravityScale = originalGravity;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
