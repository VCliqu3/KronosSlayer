using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryAchievements : MonoBehaviour
{
    public TMP_Text textTotalEnemiesKilled;
    public TMP_Text textTotalScore;

    // Start is called before the first frame update
    void Start()
    {
        textTotalEnemiesKilled.text = "Alienígenas Eliminados: " + ScoreController.CalculateTotalEnemiesKilled();
        textTotalScore.text = "Puntaje Total: " + ScoreController.CalculateTotalScore();
    }

    
    
}
