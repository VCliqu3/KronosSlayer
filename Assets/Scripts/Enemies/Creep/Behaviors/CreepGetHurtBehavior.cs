using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepGetHurtBehavior : StateMachineBehaviour
{
    private BasicEnemyMovementController _basicEnemyMovementController;
    private CreepShootController _creepShootController;
    private BasicEnemyHealthController _basicEnemyHealthController;

    private float time = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _basicEnemyMovementController = animator.gameObject.GetComponent<BasicEnemyMovementController>();
        _creepShootController = animator.gameObject.GetComponent<CreepShootController>();
        _basicEnemyHealthController = animator.gameObject.GetComponent<BasicEnemyHealthController>();

        _basicEnemyHealthController.isHurting = true;

        time = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;

        if(time >= _basicEnemyHealthController.timeHurting)
        {
            if (_creepShootController.playerOnShootRange)
            {
                animator.Play("Shoot");
            }
            else
            {
                animator.Play("Idle");
            }

            _basicEnemyHealthController.isHurting = false;
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
