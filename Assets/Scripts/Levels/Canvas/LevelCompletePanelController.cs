using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCompletePanelController : MonoBehaviour
{
    public TMP_Text enemiesKilledText;
    public TMP_Text scoreText;
    public TMP_Text timeRemainingText;

    // Start is called before the first frame update
    void Start()
    {
        SetEnemiesKilledText();
        SetScoreText();
        SetTimeRemainingText();      
    }

    public void SetEnemiesKilledText()
    {
        enemiesKilledText.text = "Alienígenas Eliminados: " + FindObjectOfType<ScoreController>().CalculateEnemiesKilledInCurrentLevel();
    }

    public void SetScoreText()
    {
        scoreText.text = "Puntaje Obtenido: " + FindObjectOfType<ScoreController>().CalculateScoreInCurrentLevel();
    }

    public void SetTimeRemainingText()
    {
        int minutes, seconds;
        ClockController _clockController = FindObjectOfType<ClockController>();

        minutes = _clockController.rawlevelTime / 60;
        seconds = _clockController.rawlevelTime - minutes * 60;

        string minutesText = minutes.ToString();
        string secondsText = seconds.ToString();

        /*
        if (minutes < 10)
        {
            minutesText = "0" + minutesText;
        }
        */

        if (seconds < 10)
        {
            secondsText = "0" + secondsText;
        }

        timeRemainingText.text =  "Tiempo Restante: "+ minutesText + ":" + secondsText;
    }
}
