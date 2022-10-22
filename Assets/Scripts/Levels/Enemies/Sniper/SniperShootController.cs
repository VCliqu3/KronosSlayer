using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperShootController : MonoBehaviour
{
    [HideInInspector]
    public LineRenderer _lineRenderer;

    public BasicEnemyMovementController _basicEnemyMovementController;

    public GameObject sniperBullet;
    public LayerMask enemyLayer;

    public float shootRange = 5f;
    public float maxShootRange = 5f;
    public float timeAiming = 2;
    public float timeRemainingAiming = 1f;
    public float bulletSpeed = 5f;
    public float bulletDamage = 3f;
    public float bulletShieldPenetration = 0f;

    public float maxDistanceLaserDraw = 100f;

    public Transform firePoint;
    public bool playerOnShootRange;
    public bool playerOnMaxShootRange;

    public bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        _basicEnemyMovementController = GetComponent<BasicEnemyMovementController>();
        _lineRenderer = GetComponent<LineRenderer>();
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
  
    public void SniperLaser()
    {
        if (Physics2D.Raycast(firePoint.position, transform.right, 1000f,~enemyLayer))
        {
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, transform.right, 1000f, ~enemyLayer);
            DrawLaser(firePoint.position, hit.point);
        }
        else
        {
            DrawLaser(firePoint.position,firePoint.position + transform.right * 20f);
        }
    }

    void DrawLaser(Vector2 startPos, Vector2 endPos) 
    {
        _lineRenderer.SetPosition(0, startPos);
        _lineRenderer.SetPosition(1, endPos);
    }
}
