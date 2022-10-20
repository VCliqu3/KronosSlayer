using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestHUD : MonoBehaviour
{
    public Image overheatBarImage;

    public Color overheatBarLowColor;
    public Color overheatBarFullColor;

    public Image dashCircleImage;
    public Image healthBar;
    public Image shieldBar;

    private HealthController _healthController;
    private MovementController _movementController;
    private RangedController _rangedController;

    void Start()
    {
        _healthController = FindObjectOfType<HealthController>();
        _movementController = FindObjectOfType<MovementController>();
        _rangedController = FindObjectOfType<RangedController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetOverheatBar();
        SetDashCooldownCircle();
        SetHealthBar();
        SetShieldBar();
    }

    public void SetOverheatBar()
    {
        overheatBarImage.fillAmount = _rangedController.overheatCounter / _rangedController.overheatLimit;
        overheatBarImage.color = Color.Lerp(overheatBarLowColor, overheatBarFullColor, overheatBarImage.fillAmount);
    }

    public void SetDashCooldownCircle()
    {
        dashCircleImage.fillAmount = _movementController.dashCooldownCounter / _movementController.dashCooldown;
    }

    public void SetHealthBar()
    {
        healthBar.fillAmount = _healthController.health / _healthController.maxHealth;
    }

    public void SetShieldBar()
    {
        shieldBar.fillAmount = _healthController.shield / _healthController.maxShield;
    }
}
