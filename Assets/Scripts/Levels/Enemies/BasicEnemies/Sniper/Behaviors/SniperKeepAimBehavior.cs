using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperKeepAimBehavior : StateMachineBehaviour
{
    private BasicEnemyMovementController _basicEnemyMovementController;
    private SniperShootController _sniperShootController;

    public float timeAim = 0f;
    private float timeRemAim = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _basicEnemyMovementController = animator.gameObject.GetComponent<BasicEnemyMovementController>();
        _sniperShootController = animator.gameObject.GetComponent<SniperShootController>();

        timeAim = 0;
        timeRemAim = 0;

        _sniperShootController._lineRenderer.enabled = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _sniperShootController.SniperLaser();

        timeAim += Time.deltaTime;

        if (_sniperShootController.playerOnMaxShootRange)
        {
            timeRemAim = 0;
        }
        else if (_basicEnemyMovementController.playerOnSight)
        {
            timeRemAim = 0;
            animator.Play("Run");
        }
        else
        {
            timeRemAim += Time.deltaTime;
        }

        if (timeAim >= _sniperShootController.timeAiming)
        {
            animator.Play("Shoot");
        }

        if (timeRemAim >= _sniperShootController.timeRemainingAiming)
        {
            animator.Play("StopAim");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _sniperShootController._lineRenderer.enabled = false;
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
