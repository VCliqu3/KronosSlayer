using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class KKHUDController : MonoBehaviour
{
    public Image healthBar;
    public Image shieldBar;
    public Image damageAccumulationBar;

    private KKHealthController _KKHealthController;


    // Start is called before the first frame update
    void Start()
    {
        _KKHealthController = FindObjectOfType<KKHealthController>();

        SetHealthBar();
        SetShieldBar();
        SetDamageAccumulationBar();
    }

    public void SetDamageAccumulationBar()
    {
        damageAccumulationBar.fillAmount = _KKHealthController.damageAccumulatedCounter / _KKHealthController.damageAccumulationLimit;
       
    }
    public void SetHealthBar()
    {
        healthBar.fillAmount = _KKHealthController.health / _KKHealthController.maxHealth;
    }

    public void SetShieldBar()
    {
        shieldBar.fillAmount = _KKHealthController.shield / _KKHealthController.maxShield;
    }
}
