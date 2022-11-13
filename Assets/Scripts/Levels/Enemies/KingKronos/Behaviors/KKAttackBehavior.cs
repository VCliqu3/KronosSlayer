using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKAttackBehavior : StateMachineBehaviour
{
    private KKMovementController _KKMovementController;
    private KKAttackController _KKAttackController;

    private float time = 0f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _KKMovementController = animator.gameObject.GetComponent<KKMovementController>();
        _KKAttackController = animator.gameObject.GetComponent<KKAttackController>();

        time = 0;

        _KKMovementController.Stop();
        _KKAttackController.isAttacking = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;

        if (time >= _KKAttackController.attackDuration)
        {
            if (_KKAttackController.playerOnAttackRange)
            {
                animator.Play("RechargeAttack");
            }
            else
            {
                animator.Play("StopAttacking");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _KKAttackController.isAttacking = false;
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
