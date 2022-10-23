using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHurtBehavior : StateMachineBehaviour
{
    private MovementController _movementController;
    private HealthController _healthController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _movementController = animator.gameObject.GetComponent<MovementController>();
        _healthController = animator.gameObject.GetComponent<HealthController>();
        
    }
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //_movementController.Stop();

        if (!_healthController.isHurting)
        {
            if (!_movementController.isGrounded)
            {
                if (ModeController.isRanged)
                {
                    animator.Play("RangedFall");
                }
                else
                {
                    animator.Play("MeleeFall");
                }
            }

            if (_movementController.velX == 0)
            {
                if (ModeController.isRanged)
                {
                    animator.Play("RangedIdle");
                }
                else
                {
                   animator.Play("MeleeIdle");
                }
            }
            else
            {
                if (ModeController.isRanged)
                {
                    animator.Play("RangedRun");
                }
                else
                {
                    animator.Play("MeleeRun");
                }
            }         
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
