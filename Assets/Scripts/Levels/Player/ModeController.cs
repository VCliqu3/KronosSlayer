using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeController : MonoBehaviour
{
    static public bool isRanged = true;

    public bool canChangeMod = true;

    private MovementController _movementController;
    private MeleeController _meleeController;
    private RangedController _rangedController;
    private HealthController _healthController;

    private LevelController _levelController;

    // Start is called before the first frame update
    void Start()
    {
        _movementController = GetComponent<MovementController>();
        _meleeController = GetComponent<MeleeController>();
        _rangedController = GetComponent<RangedController>();
        _healthController = GetComponent<HealthController>();

        _levelController = FindObjectOfType<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {              
        ChangeMode();
        EnableDisableChangeMode();
    }

    void EnableDisableChangeMode()
    {
        if(!_meleeController.isAttacking && !_meleeController.isOnAttackTransition && !_movementController.isDashing && !_healthController.isHurting && !_healthController.playerHasDied && !PauseController.gamePaused && !_levelController.levelCompleted)
        {
            canChangeMod = true;
        }
        else
        {
            canChangeMod = false;
        }
    }

    void ChangeMode()
    {

        if (Input.GetMouseButtonDown(1) && canChangeMod)
        {
            isRanged = !isRanged;          
        }

        if (isRanged)
        {
            _meleeController.attackEnable = false;
        }
        else
        {
            _rangedController.shootEnable = false;
        }

    }

}
