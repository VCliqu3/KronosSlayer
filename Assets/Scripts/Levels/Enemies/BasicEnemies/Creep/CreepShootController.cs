using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepShootController : MonoBehaviour
{
    private BasicEnemyMovementController _basicEnemyMovementController;

    public GameObject creepBullet;

    public float shootRange = 5f;
    public float maxShootRange = 5f;
    public float fireRate = 1;
    public float timeRemainingShooting = 1.5f;
    public float bulletSpeed = 5f;
    public float bulletDamage = 3f;
    public float bulletShieldPenetration = 0f;

    public Transform firePoint;
    public bool playerOnShootRange;
    public bool playerOnMaxShootRange;

    private float time;
    private float nextTimeFire;

    public bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        _basicEnemyMovementController = GetComponent<BasicEnemyMovementController>();

        time = 1 / fireRate; // Para que dispare apenas se empieze a ejecutar Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        playerOnShootRange = _basicEnemyMovementController.DetectPlayer(shootRange,"front");
        playerOnMaxShootRange = _basicEnemyMovementController.DetectPlayer(maxShootRange, "front");

        time += Time.deltaTime;
    }

    public void Shoot()
    {
        nextTimeFire = 1 / fireRate;

        isShooting = true;

        if (time >= nextTimeFire)
        {
            GameObject eBullet = Instantiate(creepBullet, firePoint.position, firePoint.rotation);

            eBullet.GetComponent<ProyectileMovementController>().speed = bulletSpeed;
            eBullet.GetComponent<ProyectileMovementController>().direction = firePoint.right;
            eBullet.GetComponent<EnemyProyectileDamageController>().damage = bulletDamage;
            eBullet.GetComponent<EnemyProyectileDamageController>().shieldPenetration = bulletShieldPenetration;

            time = 0;
        }
    }
}
