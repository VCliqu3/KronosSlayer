using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTransition31 : StateMachineBehaviour
{
    private MovementController _movementController;
    private MeleeController _meleeController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _movementController = animator.gameObject.GetComponent<MovementController>();
        _meleeController = animator.gameObject.GetComponent<MeleeController>();
        _meleeController.isOnAttackTransition = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        if (_meleeController.isAttacking)
        {
            animator.Play("Attack1");
        }
        */

        /*
        if (!_movementController.isGrounded) //Si salta
        {
            animator.Play("MeleeJump");
        }
        */

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _meleeController.isOnAttackTransition = false;
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
