using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KKTPController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private KKMovementController _KKMovementController;

    public float TPRangeMin = 5f;
    public float TPRangeMax = 7f;

    public bool playerOnTPRange = false;

    public bool TPEnabled = true;

    public float TPAttackDamage = 3f;
    public float TPAttacShieldPenetration = 0f;

    public float timeStayingUp = 1f;
    public float distanceToAppearUp = 2f;
    public float downImpulse = 2f;
    public float TPCooldown = 5f;
    public float TPCooldownCounter;

    public bool isTPing = false;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _KKMovementController = GetComponent<KKMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerOnTPRange = (_KKMovementController.DetectPlayer(TPRangeMax, "front") && !_KKMovementController.DetectPlayer(TPRangeMin, "front"));
        //TPAttack();
    }

    void TPAttack()
    {
        if (TPEnabled && playerOnTPRange)
        {
            StartCoroutine(TPAttacking());
            StartCoroutine(TPAttackCooldown());
        }
    }

    IEnumerator TPAttacking()
    {
        TPEnabled = false;
        isTPing = true;
        TPCooldownCounter = 0;

        Vector2 playerPos = FindObjectOfType<MovementController>().transform.position;
        transform.position = new Vector2(playerPos.x, playerPos.y + distanceToAppearUp);

        float originalGravity = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = 0f;

        yield return new WaitForSeconds(timeStayingUp);

        _rigidbody2D.gravityScale = originalGravity;

        _rigidbody2D.AddForce(new Vector2(0,-downImpulse), ForceMode2D.Impulse); 


    }
    IEnumerator TPAttackCooldown()
    {
        while (TPCooldownCounter < TPCooldown)
        {
            TPCooldownCounter += Time.deltaTime;

            yield return null;
        }

        TPCooldownCounter = TPCooldownCounter > TPCooldown ? TPCooldown : TPCooldown;

        TPEnabled = true;
    }
}
