using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKIdleBehavior : StateMachineBehaviour
{
    private KKMovementController _KKMovementController;
    private KKDashController _KKDashController;
    private KKJumpAttackController _KKJumpAttackController;
    private KKTPController _KKTPController;
    private KKHealthController _KKHealthController;
    private KKAttackController _KKAttackController;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _KKMovementController = animator.gameObject.GetComponent<KKMovementController>();
        _KKDashController = animator.gameObject.GetComponent<KKDashController>();
        _KKJumpAttackController = animator.gameObject.GetComponent<KKJumpAttackController>();
        _KKTPController = animator.gameObject.GetComponent<KKTPController>();
        _KKHealthController = animator.gameObject.GetComponent<KKHealthController>();
        _KKAttackController = animator.gameObject.GetComponent<KKAttackController>();

        _KKAttackController.isAttacking = false;
        _KKMovementController.Stop();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_KKMovementController.isActivated)
        {
            if(_KKHealthController.shield <=0 && !_KKHealthController.isEnraged)
            {
                _KKHealthController.Enrage();
                animator.Play("Enrage");
            }
            else if (_KKMovementController.playerOnSight)
            {
                if (_KKTPController.TPEnabled && _KKHealthController.damageAccumulatedCounter >= _KKHealthController.damageAccumulationLimit)
                {
                    _KKTPController.TPAttack();
                }
                else if (_KKJumpAttackController.playerOnJumpRange && _KKJumpAttackController.jumpEnabled)
                {
                    _KKJumpAttackController.Jump();
                }
                else if (_KKDashController.playerOnDashRange && _KKDashController.dashEnabled)
                {
                    _KKDashController.Dash();
                }
                else
                {
                    animator.Play("Run");
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
