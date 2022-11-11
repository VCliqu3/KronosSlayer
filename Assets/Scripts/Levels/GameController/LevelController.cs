using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelController : MonoBehaviour
{
    static public int numberOfLevels = 3;
    static public int level1BuildIndex = 2;

    static public int[] numberOfAttemps = new int[numberOfLevels]; //Hay 3 niveles

    public float timeForLevelText = 1f;
    public TMP_Text levelText;

    public GameObject deathPanel;
    public GameObject levelCompletePanel;

    public bool levelCompleted = false;

    void Start()
    {        
        ChangeAttempsInCurrentLevel(1);

        if (CalculateAttempsInCurrentLevel() == 1)
        {
            StartCoroutine(FadeInLevelText(timeForLevelText));
        }
    }
    public void ChangeAttempsInCurrentLevel(int quantity)
    {
        numberOfAttemps[SceneManager.GetActiveScene().buildIndex - level1BuildIndex] += quantity; //El buildIndex del Level1 es 1
    }

    public int CalculateAttempsInCurrentLevel()
    {
        int attemps = numberOfAttemps[SceneManager.GetActiveScene().buildIndex - level1BuildIndex];

        return attemps;
    }

    public void ResetAttempsInAllLevels()
    {
        for (int i = 0; i < numberOfAttemps.Length; i++)
        {
            numberOfAttemps[i] = 0; //Resetea el numero de intentos de todo el arreglo numberOfAttemps (todos los niveles)
        }

    }
    public IEnumerator FadeInLevelText(float time)
    {
        yield return new WaitForSeconds(time);
        levelText.text = "Nivel " + (SceneManager.GetActiveScene().buildIndex - level1BuildIndex + 1);
        levelText.gameObject.SetActive(true);

    }

    public void ActivateDeathPanel()
    {
        deathPanel.SetActive(true);
    }

    public void ActivateLevelCompletePanel()
    {
        levelCompleted = true;
        levelCompletePanel.SetActive(true);

        Time.timeScale = 0f;
        PauseController.gamePaused = true;
    }

}
