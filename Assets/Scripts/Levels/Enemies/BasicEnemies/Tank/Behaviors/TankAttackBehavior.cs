using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttackBehavior : StateMachineBehaviour
{
    private BasicEnemyMovementController _basicEnemyMovementController;
    private TankAttackController _tankAttackController;

    private float time = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _basicEnemyMovementController = animator.gameObject.GetComponent<BasicEnemyMovementController>();
        _tankAttackController = animator.gameObject.GetComponent<TankAttackController>();

        time = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _basicEnemyMovementController.Stop();

        time += Time.deltaTime;

        if (time >= _tankAttackController.attackDuration)
        {
            if (_tankAttackController.playerOnAttackRange)
            {
                animator.Play("RechargeAttack");
            }
            else
            {
                animator.Play("StopAttacking");
            }

            /*
             * else if (_basicEnemyMovementController.playerOnSight && _basicEnemyMovementController.groundDown && !_basicEnemyMovementController.groundInFront)
            {
                animator.Play("Run");
            }
            */
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
