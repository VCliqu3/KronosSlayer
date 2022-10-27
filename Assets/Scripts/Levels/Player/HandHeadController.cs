using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHeadController : MonoBehaviour
{
    [HideInInspector]
    public Vector2 mousePos;
    [HideInInspector]
    public Vector2 playerPos;
    [HideInInspector]
    public float aimAngle;

    public Camera _camera;
    public Transform hand;
    public Transform head;
    public Transform bodyCenter;

    private MovementController _movementController;
    private HealthController _healthController;
    private MeleeController _meleeController;

    public float handDistance;
 
    // Start is called before the first frame update
    void Start()
    {
        _movementController = GetComponent<MovementController>();
        _healthController = GetComponent<HealthController>();
        _meleeController = GetComponent<MeleeController>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = GetMousePosition();
        playerPos = GetPlayerPosition();
        aimAngle = CalculateAimAngle(mousePos, playerPos);

        FlipHand();
        FlipHead();
    }

    Vector2 GetMousePosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        return mousePosition;
    }

    Vector2 GetPlayerPosition()
    {
        Vector2 playerPosition = _camera.WorldToScreenPoint(bodyCenter.position);
        return playerPosition;
    }

    float CalculateAimAngle(Vector2 mousePosition, Vector2 playerPosition)
    {
        float angle;

        if (_movementController.playerFacingRight)
        {
            angle = Mathf.Atan2(mousePosition.y - playerPosition.y, mousePosition.x - playerPosition.x);
        }
        else
        {
            angle = Mathf.Atan2(mousePosition.y - playerPosition.y, playerPosition.x - mousePosition.x);
        }

        angle *= 180 / Mathf.PI;
        angle = angle < 0 ? angle + 360 : angle;

        return angle;
    }

    void FlipHand()
    {
        if(_meleeController.isAttacking || _meleeController.isOnAttackTransition ||_movementController.isDashing || _healthController.isHurting) //|| _healthController.playerIsDead || ModeController.isRanged
        {
            hand.localPosition = handDistance * new Vector2(1, 0);
            hand.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            hand.localPosition = handDistance * new Vector2(Mathf.Cos(aimAngle * Mathf.PI / 180), Mathf.Sin(aimAngle * Mathf.PI / 180));

            if (aimAngle >= 90 && aimAngle <= 270)
            {
                hand.localRotation = Quaternion.Euler(0, 180, 180 - aimAngle);
            }
            else
            {
                hand.localRotation = Quaternion.Euler(0, 0, aimAngle);
            }
        }
    
    }

    void FlipHead()
    {
        if (_meleeController.isAttacking || _meleeController.isOnAttackTransition || _movementController.isDashing || _healthController.isHurting) //|| _healthController.playerIsDead || ModeController.isRanged
        {
            head.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            if (aimAngle >= 90 && aimAngle <= 270)
            {
                head.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                head.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
