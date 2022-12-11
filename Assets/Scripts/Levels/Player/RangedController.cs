using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedController : MonoBehaviour
{
    private MovementController _movementController;
    private HealthController _healthController;
    private HandHeadController _handHeadController;

    private float time;
    private float nextTimeFire;

    public Transform firePoint;
    public GameObject playerBullet;

    public float fireRate;
    public bool enableBulletDispersion;
    public float dispersionFactor; //Factor de dispersion de las balas

    public bool shootEnable;
    public bool isShooting;

    public float bulletDamage = 1f;
    public float bulletShieldPenetration = 0f;
    public float bulletSpeed = 5f;

    public float overheatLimit; //Limite de la barra de sobrecalentamiento (u)
    public float overheatIncreaseMultiplier =1; //Velocidad a la que se llena al disparar (u/s)
    public float overheatDecreaseMultiplier =1; //Velocidad a la que se vacia al no disparar (u/s)
    public float overheatCoolingDecreaseMultiplier = 1; //Velocidad a la que se vacia al sobrecalentarse (u/s)
    public float timeAtMaxOverheat; //Tiempo que permanece llena cuando se sobrecalienta
    public float timeAtMinOverheat; //Tiempo que permanece vacia cuando se sobrecalienta
    public float timeForNaturalCooling = 1f;

    public float overheatCounter;
    public bool gunIsOverheated;

    private HUDController _HUDController;

    private LevelController _levelController;

    //SFX

    public string nameSFXPlayerShoot;
    public string nameSFXPlayerGunOverheated;
    public string nameSFXPlayerGunOverheatEnd;

    // Start is called before the first frame update
    void Start()
    {
        _movementController = GetComponent<MovementController>();
        _healthController = GetComponent<HealthController>();
        _handHeadController = GetComponent<HandHeadController>();

        _HUDController = FindObjectOfType<HUDController>();
        //_HUDController.SetOverheatBar();

        _levelController = FindObjectOfType<LevelController>();

        time = 1 / fireRate;
    }

    // Update is called once per frame
    void Update()
    {
       
        Shoot();
        GunOverheat();
        EnableDisableShooting();

        time += Time.deltaTime;
    }

    void EnableDisableShooting()
    {
        if (ModeController.isRanged && !gunIsOverheated && !_movementController.isDashing && !_healthController.isHurting && !_healthController.playerHasDied && !PauseController.gamePaused && !PauseButonController.mouseOnPauseButton && !_levelController.levelCompleted) //&& !PauseButonController.mouseOnPauseButton
        {
            shootEnable = true;
        }
        else
        {
            shootEnable = false;
        }   
    }

    void Shoot()
    {
        nextTimeFire = 1 / fireRate;

        if (Input.GetMouseButton(0) && shootEnable)
        {
            isShooting = true;

            if (time >= nextTimeFire)
            {
                Vector2 shootDir = new(Mathf.Cos(_handHeadController.aimAngle * Mathf.PI / 180), Mathf.Sin(_handHeadController.aimAngle * Mathf.PI / 180));

                if (!_movementController.playerFacingRight)
                {
                    shootDir.x *= -1;
                }

                if (enableBulletDispersion)
                {
                    Vector2 dispersion = new(Random.Range(-dispersionFactor, dispersionFactor), Random.Range(-dispersionFactor, dispersionFactor));

                    shootDir += dispersion;
                }

                shootDir.Normalize();

                GameObject pBullet = Instantiate(playerBullet, firePoint.position, firePoint.rotation);

                pBullet.GetComponent<ProyectileMovementController>().direction = shootDir;
                pBullet.GetComponent<ProyectileMovementController>().speed = bulletSpeed;
                pBullet.GetComponent<PlayerProyectileDamageController>().damage = bulletDamage;
                pBullet.GetComponent<PlayerProyectileDamageController>().shieldPenetration = bulletShieldPenetration;

                AudioManager.instance.PlaySFX(nameSFXPlayerShoot);

                time = 0;
            }

        }
        else
        {
            isShooting = false;
        }
    }

    void GunOverheat()
    {
        if (isShooting)
        {
            overheatCounter += Time.deltaTime*overheatIncreaseMultiplier;

            overheatCounter = overheatCounter > overheatLimit ? overheatLimit : overheatCounter;

            _HUDController.SetOverheatBar();

        }
        else if(!isShooting && !gunIsOverheated && overheatCounter > 0 && time>=timeForNaturalCooling)
        {
            overheatCounter -= Time.deltaTime * overheatDecreaseMultiplier;

            overheatCounter = overheatCounter < 0 ? 0 : overheatCounter;

            _HUDController.SetOverheatBar();
        }

        if (overheatCounter >= overheatLimit)
        {
            if (!gunIsOverheated)
            {
                gunIsOverheated = true;

                AudioManager.instance.PlaySFX(nameSFXPlayerGunOverheated);

                StartCoroutine(CoolGunDown());
            }
        }
    }

    IEnumerator CoolGunDown()
    {
        yield return new WaitForSeconds(timeAtMaxOverheat);

        while (overheatCounter > 0)
        {
            //overheatIndicator -= 0.01f * overheatCoolingDecreaseMultiplier;
            //yield return new WaitForSeconds(0.01f);

            overheatCounter -= Time.deltaTime * overheatCoolingDecreaseMultiplier;
            _HUDController.SetOverheatBar();

            yield return null;
        }

        overheatCounter = overheatCounter < 0 ? 0 : overheatCounter;

        _HUDController.SetOverheatBar();

        yield return new WaitForSeconds(timeAtMinOverheat);

        gunIsOverheated = false;

        AudioManager.instance.PlaySFX(nameSFXPlayerGunOverheatEnd);
    }
}
