using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedRunBehavior : StateMachineBehaviour
{
    private MovementController _movementController;
    private RangedController _rangedController;

    public string WalkingSFXName;
    private bool isPlayingWalkingSFX = false;

    public string footstep1SFXName;
    public string footstep2SFXName;

    public bool individualFootsteps;

    public float timeBeetweenFootsteps = 0.2f;
    public float timeforFirtsFootstep = 0.2f;

    private float time = 0f;

    private int footstepNumber = 1;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _movementController = animator.gameObject.GetComponent<MovementController>();
        _rangedController = animator.gameObject.GetComponent<RangedController>();

        time = timeBeetweenFootsteps-timeforFirtsFootstep;
        footstepNumber = 1;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (individualFootsteps)
        {
            time += Time.deltaTime;

            if (time >= timeBeetweenFootsteps)
            {
                time = 0;
                switch (footstepNumber)
                {
                    case 1:
                        AudioManager.instance.PlaySFX(footstep1SFXName);
                        footstepNumber = 2;
                        break;
                    case 2:
                        AudioManager.instance.PlaySFX(footstep2SFXName);
                        footstepNumber = 1;
                        break;
                    default:
                        AudioManager.instance.PlaySFX(footstep1SFXName);
                        footstepNumber = 2;
                        break;
                }
            }

        }
        else
        {
            if (!isPlayingWalkingSFX)
            {
                AudioManager.instance.PlayPerpetualSFX(WalkingSFXName);
                isPlayingWalkingSFX = true;
            }
        }

        if (PauseController.gamePaused)
        {
            AudioManager.instance.StopPerpetualSFX();
            isPlayingWalkingSFX = false;
        }

        if(_movementController.velX == 0 || _movementController.groundInFront)
        {
            animator.Play("RangedIdle");
        }

        if (!_movementController.isGrounded)
        {
            animator.Play("RangedJump");
        }

        if (!ModeController.isRanged)
        {
            animator.Play("MeleeRun");
        }

        if (_rangedController.isShooting)
        {         
            animator.Play("RunShoot");     
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.instance.StopPerpetualSFX();
        AudioManager.instance.StopPerpetualSFX(); isPlayingWalkingSFX = false;

        time = 0f;
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
