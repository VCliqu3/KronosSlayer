using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyHealthBarController : MonoBehaviour
{
    private BasicEnemyHealthController _basicEnemyHealthController;
    private BasicEnemyMovementController _basicEnemyMovementController;
    public GameObject healthBarsCanvas;
    public Image healthBar;
    public Image shieldBar;
    public bool facingRight;

    void Start()
    {
        _basicEnemyHealthController = GetComponent<BasicEnemyHealthController>();
        _basicEnemyMovementController = GetComponent<BasicEnemyMovementController>();
        healthBarsCanvas.SetActive(false);

    }
    void Update()
    {
        if (_basicEnemyHealthController.shield != _basicEnemyHealthController.maxShield || _basicEnemyHealthController.health != _basicEnemyHealthController.maxHealth)
        {
            healthBarsCanvas.SetActive(true);
        }

        RotateCanvas();
    }
    public void SetHealthBar()
    {
        healthBar.fillAmount = _basicEnemyHealthController.health / _basicEnemyHealthController.maxHealth;
    }
    public void SetShieldBar()
    {
        shieldBar.fillAmount = _basicEnemyHealthController.shield / _basicEnemyHealthController.maxShield;
    }
    public void CheckIfHasRotated()
    {

    }
    public void RotateCanvas()
    {
        healthBarsCanvas.GetComponent<RectTransform>().localRotation = transform.rotation;   
    }

}