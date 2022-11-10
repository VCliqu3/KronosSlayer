using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    public static int[] score = new int[LevelController.numberOfLevels]; //Hay 3 niveles 
    public static int[] enemiesKilled = new int[LevelController.numberOfLevels]; //Hay 3 niveles 
    
    void Start()
    {
        ResetScoreInCurrentLevel();
        ResetEnemiesKilledInCurrentLevel();
    }

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

    public void AddEnemiesKilledInCurrentLevel(int numberOfEnemies) //Metodo para añadir puntaje
    {
        enemiesKilled[SceneManager.GetActiveScene().buildIndex - LevelController.level1BuildIndex] += numberOfEnemies; //El buildIndex del Level1 es 1
    }

    public void ResetEnemiesKilledInCurrentLevel()
    {
        enemiesKilled[SceneManager.GetActiveScene().buildIndex - LevelController.level1BuildIndex] = 0; //El buildIndex del Level1 es 1
    }

    public void ResetEnemiesKilledInAllLevels()
    {
        for (int i = 0; i < enemiesKilled.Length; i++)
        {
            enemiesKilled[i] = 0; //Resetea el score de todo el arreglo score (todos los niveles)
        }
    }
    public int CalculateEnemiesKilledInCurrentLevel()
    {
        int enemiesKilledInLevel = enemiesKilled[SceneManager.GetActiveScene().buildIndex - LevelController.level1BuildIndex];

        return enemiesKilledInLevel;
    }

    public int CalculateTotalEnemiesKilled()
    {
        int totalEnemiesKilled = 0;

        for (int i = 0; i < enemiesKilled.Length; i++)
        {
            totalEnemiesKilled += enemiesKilled[i];
        }

        return totalEnemiesKilled;
    }
}
