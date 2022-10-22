using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperIdleBehavior : StateMachineBehaviour
{
    private BasicEnemyMovementController _basicEnemyMovementController;
    private SniperShootController _sniperShootController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _basicEnemyMovementController = animator.gameObject.GetComponent<BasicEnemyMovementController>();
        _sniperShootController = animator.gameObject.GetComponent<SniperShootController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_basicEnemyMovementController.isPatrolling)
        {
            _basicEnemyMovementController.isPatrolling = true;
            _basicEnemyMovementController.patrolCoroutine = _basicEnemyMovementController.Patrol();
            _basicEnemyMovementController.StartPatrol();
        }

        if (_sniperShootController.playerOnShootRange)
        {
            _basicEnemyMovementController.StopPatrol();
            animator.Play("Aim");
        }
        else if (_basicEnemyMovementController.playerOnSight && _basicEnemyMovementController.groundDown && !_basicEnemyMovementController.groundInFront)
        {
            _basicEnemyMovementController.StopPatrol();
            animator.Play("Run");
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
