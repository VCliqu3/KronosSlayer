using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    public static int[] score = new int[LevelController.numberOfLevels]; //Hay 3 niveles 

    public void AddScoreInCurrentLevel(int scoreToAdd) //Metodo para añadir puntaje
    {
        score[SceneManager.GetActiveScene().buildIndex - LevelController.level1BuildIndex] += scoreToAdd; //El buildIndex del Level1 es 1
    }

    public void ResetScoreInCurrentLevel()
    {
        score[SceneManager.GetActiveScene().buildIndex - LevelController.level1BuildIndex] = 0; //El buildIndex del Level1 es 1
    }

    public void ResetScoreInAllLevels()
    {
        for (int i = 0; i < score.Length; i++)
        {
            score[i] = 0; //Resetea el score de todo el arreglo score (todos los niveles)
        }
    }

    public int CalculateScoreInCurrentLevel()
    {
        int scoreInLevel = score[SceneManager.GetActiveScene().buildIndex - LevelController.level1BuildIndex];

        return scoreInLevel;
    }

    public int CalculateTotalScore()
    {
        int totalScore = 0;

        for (int i = 0; i < score.Length; i++)
        {
            totalScore += score[i];
        }

        return totalScore;
    }
}
