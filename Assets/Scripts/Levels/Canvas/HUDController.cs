using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    public Image overheatBarImage;

    public Color overheatBarLowColor;
    public Color overheatBarFullColor;

    public Image dashCooldownIndicator;
    public Image healthBar;
    public Image shieldBar;
    public TMP_Text scoreText;
    public TMP_Text timeText;

    private HealthController _healthController;
    private MovementController _movementController;
    private RangedController _rangedController;

    void Start()
    {
        _healthController = FindObjectOfType<HealthController>();
        _movementController = FindObjectOfType<MovementController>();
        _rangedController = FindObjectOfType<RangedController>();

        SetOverheatBar();
        SetDashCooldownIndicator();
        SetHealthBar();
        SetShieldBar();
        SetScoreText();
    }
   
    public void SetOverheatBar()
    {
        overheatBarImage.fillAmount = _rangedController.overheatCounter / _rangedController.overheatLimit;
        overheatBarImage.color = Color.Lerp(overheatBarLowColor, overheatBarFullColor, overheatBarImage.fillAmount);
    }

    public void SetDashCooldownIndicator()
    {
        dashCooldownIndicator.fillAmount = _movementController.dashCooldownCounter / _movementController.dashCooldown;
    }

    public void SetHealthBar()
    {
        healthBar.fillAmount = _healthController.health / _healthController.maxHealth;
    }

    public void SetShieldBar()
    {
        shieldBar.fillAmount = _healthController.shield / _healthController.maxShield;
    }

    public void SetScoreText()
    {
        scoreText.text = "Puntaje: " + FindObjectOfType<ScoreController>().CalculateTotalScore();
    }

    public void SetTimeText()
    {
        int minutes, seconds;
        ClockController _clockController = FindObjectOfType<ClockController>();
        minutes = _clockController.rawlevelTime / 60;
        seconds = _clockController.rawlevelTime - minutes * 60;

        string minutesText = minutes.ToString();
        string secondsText = seconds.ToString();

        if (minutes < 10)
        {
            minutesText = "0" + minutesText;
        }

        if (seconds < 10)
        {
            secondsText = "0" + secondsText;
        }

        timeText.text = minutesText + ":" + secondsText;
    }
}
