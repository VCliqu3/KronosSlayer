using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKRunBehavior : StateMachineBehaviour
{
    private KKMovementController _KKMovementController;
    private KKDashController _KKDashController;
    private KKJumpAttackController _KKJumpAttackController;
    private KKTPController _KKTPController;
    private KKAttackController _KKAttackController;
    private KKHealthController _KKHealthController;

    public float time = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _KKMovementController = animator.gameObject.GetComponent<KKMovementController>();
        _KKDashController = animator.gameObject.GetComponent<KKDashController>();
        _KKJumpAttackController = animator.gameObject.GetComponent<KKJumpAttackController>();
        _KKTPController = animator.gameObject.GetComponent<KKTPController>();
        _KKAttackController = animator.gameObject.GetComponent<KKAttackController>();
        _KKHealthController = animator.gameObject.GetComponent<KKHealthController>();

        time = 0f;    
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _KKMovementController.Run();

        if (_KKHealthController.shield <= 0 && !_KKHealthController.isEnraged)
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
            else if (_KKAttackController.playerOnAttackRange)
            {
                animator.Play("Attack");
            }
        }
        else
        {
            time += Time.deltaTime;
        }

        if (time >= _KKMovementController.timeRemainingFollowing)
        {
            time = 0;
            animator.Play("Idle");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _KKMovementController.Stop();
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
