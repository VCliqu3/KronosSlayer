using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeIdleBehavior : StateMachineBehaviour
{
    private MovementController _movementController;
    private MeleeController _meleeController;

    private LevelController _levelController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _movementController = animator.gameObject.GetComponent<MovementController>();
        _meleeController = animator.gameObject.GetComponent<MeleeController>();

        _levelController = FindObjectOfType<LevelController>();

        _meleeController.attackingBehind = false;              
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (_movementController.velX != 0 && !_movementController.groundInFront && !_levelController.levelCompleted) //Si Corre
        {
            animator.Play("MeleeRun");
        }

        if (!_movementController.isGrounded) //Si salta
        {
            animator.Play("MeleeJump");
        }

        if (ModeController.isRanged) //Si cambia a Rango
        {
            animator.Play("RangedIdle");
        }

        if (_meleeController.isAttacking)
        {
            animator.Play("Attack1");
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
