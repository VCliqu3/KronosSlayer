using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGetHurtBehavior : StateMachineBehaviour
{
    private BasicEnemyMovementController _basicEnemyMovementController;
    private TankAttackController _tankAttackController;
    private BasicEnemyHealthController _basicEnemyHealthController;

    private float time = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _basicEnemyMovementController = animator.gameObject.GetComponent<BasicEnemyMovementController>();
        _tankAttackController = animator.gameObject.GetComponent<TankAttackController>();
        _basicEnemyHealthController = animator.gameObject.GetComponent<BasicEnemyHealthController>();

        _basicEnemyHealthController.isHurting = true;

        time = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _basicEnemyMovementController.Stop();

        time += Time.deltaTime;

        if (time >= _basicEnemyHealthController.timeHurting)
        {
            if (_tankAttackController.playerOnAttackRange)
            {
                animator.Play("Attack");
            }
            else if (_basicEnemyMovementController.playerOnSight)
            {
                animator.Play("Run");
            }
            else
            {
                animator.Play("Idle");
            }        
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _basicEnemyHealthController.isHurting = false;
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
