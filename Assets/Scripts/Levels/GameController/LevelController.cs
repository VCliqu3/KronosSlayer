using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelController : MonoBehaviour
{
    static public int numberOfLevels = 4;
    static public int level1BuildIndex = 2;

    static public int[] numberOfAttemps = new int[numberOfLevels]; //Hay 3 niveles

    static public int currentLevelIndex;
    public float timeForLevelText = 1f;
    public TMP_Text levelText;

    public GameObject deathPanel;
    public GameObject levelCompletePanel;
    public GameObject denyNextLevelPanel;
    public TMP_Text denyNextLevelPanelText;
    public float timeDenyNextLevelPanelActive = 2f;

    public bool levelCompleted = false;

    void Start()
    {
        CursorController.onGameplay = true;
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

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

    static public void ResetAttempsInAllLevels()
    {
        for (int i = 0; i < numberOfAttemps.Length; i++)
        {
            numberOfAttemps[i] = 0; //Resetea el numero de intentos de todo el arreglo numberOfAttemps (todos los niveles)
        }
    }

    static public int CalculateTotalSAttemps()
    {
        int totalAttemps = 0;

        for (int i = 0; i < numberOfAttemps.Length; i++)
        {
            totalAttemps += numberOfAttemps[i];
        }

        return totalAttemps;
    }

    public IEnumerator FadeInLevelText(float time)
    {
        yield return new WaitForSeconds(time);

        if ((SceneManager.GetActiveScene().buildIndex) == level1BuildIndex + 3) //Si es el nivel 4
        {
            levelText.text = "King Kronos";
        }
        else
        {
            levelText.text = "Nave " + (SceneManager.GetActiveScene().buildIndex - level1BuildIndex + 1);
        }
        levelText.gameObject.SetActive(true);

    }

    public void ActivateDeathPanel()
    {
        deathPanel.SetActive(true);
        CursorController.onGameplay = false;

        FindObjectOfType<ClockController>().levelTimeCanDecrease = false;
        FindObjectOfType<ClockController>().shipCanExplode = false;
    }

    public void ActivateLevelCompletePanel()
    {
        levelCompleted = true;
        levelCompletePanel.SetActive(true);

        
        Time.timeScale = 0f;
        PauseController.gamePaused = true;
        

        CursorController.onGameplay = false;

        FindObjectOfType<ClockController>().levelTimeCanDecrease = false;
        FindObjectOfType<ClockController>().shipCanExplode = false;
    }

    public void ActivateDenyNextLevelPanel()
    {
        denyNextLevelPanel.SetActive(true);
    }

    public IEnumerator NextLevelLogic()
    {
        GameObject[] basicEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        int numberEnemiesRemaining = basicEnemies.Length;

        if(numberEnemiesRemaining <= 0)
        {
            ActivateLevelCompletePanel();
        }
        else
        {
            if (!denyNextLevelPanel.activeInHierarchy)
            {
                ActivateDenyNextLevelPanel();
                if (numberEnemiesRemaining > 1)
                {
                    denyNextLevelPanelText.text = "Aun quedan " + numberEnemiesRemaining + " enemigos por eliminar.";
                }
                else
                {
                    denyNextLevelPanelText.text = "Aun queda " + numberEnemiesRemaining + " enemigo por eliminar.";
                }
                denyNextLevelPanel.GetComponent<Animator>().SetTrigger("PopUp");
                yield return new WaitForSeconds(timeDenyNextLevelPanelActive);
                denyNextLevelPanel.GetComponent<Animator>().SetTrigger("Close");
                yield return new WaitUntil(() => denyNextLevelPanelText.fontSize == 0); //Se espera que se haya cerrado si el tama�o del texto es 0;
                denyNextLevelPanel.SetActive(false);
            }
        }

    }
}
