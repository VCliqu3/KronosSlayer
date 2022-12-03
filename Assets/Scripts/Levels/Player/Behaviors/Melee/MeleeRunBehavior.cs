using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeRunBehavior : StateMachineBehaviour
{
    private MovementController _movementController;
    private MeleeController _meleeController;

    private LevelController _levelController;

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
        _meleeController = animator.gameObject.GetComponent<MeleeController>();

        _levelController = FindObjectOfType<LevelController>();

        time = timeBeetweenFootsteps - timeforFirtsFootstep;
        footstepNumber = 1; 
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (individualFootsteps)
        {
            time += Time.deltaTime;

            if(time >= timeBeetweenFootsteps && !_levelController.levelCompleted)
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

        if (_movementController.velX == 0 || _movementController.groundInFront || _levelController.levelCompleted)
        {
            animator.Play("MeleeIdle");
        }

        if (!_movementController.isGrounded)
        {
            animator.Play("MeleeJump");
        }

        if (ModeController.isRanged)
        {
            animator.Play("RangedRun");
        }

        if (_meleeController.isAttacking)
        {
            animator.Play("Attack1");
        }
    }

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
