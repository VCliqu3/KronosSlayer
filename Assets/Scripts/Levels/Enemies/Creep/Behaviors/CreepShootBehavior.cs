using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepShootBehavior : StateMachineBehaviour
{
    private BasicEnemyMovementController _basicEnemyMovementController;
    private CreepShootController _creepShootController;

    private float time = 0f;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _basicEnemyMovementController = animator.gameObject.GetComponent<BasicEnemyMovementController>();
        _creepShootController = animator.gameObject.GetComponent<CreepShootController>();
   
        if (!_creepShootController.playerOnMaxShootRange && _basicEnemyMovementController.playerOnSight && _basicEnemyMovementController.groundDown && !_basicEnemyMovementController.groundInFront)
        {
            animator.Play("Run");
        }

        time = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _creepShootController.Shoot();

        if (_creepShootController.playerOnMaxShootRange)
        {
            time = 0;
        }
        else if (_basicEnemyMovementController.playerOnSight)
        {
            time = 0;
            animator.Play("Run");
        }
        else
        {
            time += Time.deltaTime;
        }

        if(time>= _creepShootController.timeRemainingShooting)
        {
            animator.Play("StopAim");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _creepShootController.isShooting = false;
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

    public void ToStopAim()
    {
        _basicEnemyMovementController._animator.Play("StopAim");
    }
}
