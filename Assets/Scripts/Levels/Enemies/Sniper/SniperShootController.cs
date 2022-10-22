using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperShootController : MonoBehaviour
{
    private Animator _animator;
    public BasicEnemyMovementController _basicEnemyMovementController;

    public GameObject sniperBullet;

    public float shootRange = 5f;
    public float maxShootRange = 5f;
    public float timeAiming = 2;
    public float timeRemainingAiming = 1f;
    public float bulletSpeed = 5f;
    public float bulletDamage = 3f;
    public float bulletShieldPenetration = 0f;

    public Transform firePoint;
    public bool playerOnShootRange;
    public bool playerOnMaxShootRange;

    public bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _basicEnemyMovementController = GetComponent<BasicEnemyMovementController>();

    }

    // Update is called once per frame
    void Update()
    {
        playerOnShootRange = _basicEnemyMovementController.DetectPlayer(shootRange, "front");
        playerOnMaxShootRange = _basicEnemyMovementController.DetectPlayer(maxShootRange, "front");
    }

    public void Shoot()
    {
        GameObject eBullet = Instantiate(sniperBullet, firePoint.position, firePoint.rotation);

        eBullet.GetComponent<ProyectileMovementController>().speed = bulletSpeed;
        eBullet.GetComponent<ProyectileMovementController>().direction = firePoint.right;
        eBullet.GetComponent<EnemyProyectileDamageController>().damage = bulletDamage;
        eBullet.GetComponent<EnemyProyectileDamageController>().shieldPenetration = bulletShieldPenetration;   
    }

    public void PlayStopAimAnimation() //Sera invocado por el CreepShootBehavior
    {
        _animator.Play("StopAim");
    }
}
