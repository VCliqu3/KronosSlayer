using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRunBehavior : StateMachineBehaviour
{
    private BasicEnemyMovementController _basicEnemyMovementController;
    private TankAttackController _tankAttackController;

    public float time = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _basicEnemyMovementController = animator.gameObject.GetComponent<BasicEnemyMovementController>();
        _tankAttackController = animator.gameObject.GetComponent<TankAttackController>();

        time = 0f;
}

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _basicEnemyMovementController.Run();

        if (_tankAttackController.playerOnAttackRange)
        {
            animator.Play("Attack");
        }
        else if(!_basicEnemyMovementController.playerOnSight)
        {
            time += Time.deltaTime;
        }

        if (time >=_tankAttackController.timeRemainingFollowing || !_basicEnemyMovementController.groundDown || _basicEnemyMovementController.groundInFront)
        {
            time = 0;
            animator.Play("Idle");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _basicEnemyMovementController.Stop();
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
