using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedRunBehavior : StateMachineBehaviour
{
    private MovementController _movementController;
    private RangedController _rangedController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _movementController = animator.gameObject.GetComponent<MovementController>();
        _rangedController = animator.gameObject.GetComponent<RangedController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_movementController.velX == 0)
        {
            animator.Play("RangedIdle");
        }

        if (!_movementController.isGrounded)
        {
            animator.Play("RangedJump");
        }

        if (!ModeController.isRanged)
        {
            animator.Play("MeleeRun");
        }

        if (_rangedController.isShooting)
        {         
            animator.Play("RunShoot");     
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
