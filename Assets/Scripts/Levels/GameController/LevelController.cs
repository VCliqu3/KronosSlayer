using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelController : MonoBehaviour
{
    static public int numberOfLevels = 5;
    static public int level1BuildIndex = 2;

    static public int[] numberOfAttemps = new int[numberOfLevels]; //Hay 3 niveles

    static public int currentLevelIndex = level1BuildIndex;
    public float timeForLevelText = 1f;
    public TMP_Text levelText;

    public GameObject deathPanel;
    public GameObject levelCompletePanel;
    public GameObject denyNextLevelPanel;
    public TMP_Text denyNextLevelPanelText;
    public float timeDenyNextLevelPanelActive = 2f;

    public bool levelCompleted = false;

    public float timeToPopUpLevelCompletePanel = 1f;

    private ClockController _clockController;

    public GameObject startGate;
    public GameObject endGate;

    public float timeForStartGateClose;

    //SFX

    public string nameSFXdenyLevelCompletePanel;
    public string nameSFXlevelCompletePanel;
    public string nameSFXdeathPanel;
    public string nameSFXgateOpen;
    public string nameSFXgateClose;

    void Start()
    {
        _clockController = GetComponent<ClockController>();

        CursorController.onGameplay = true;
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        ChangeAttempsInCurrentLevel(1);

        if (CalculateAttempsInCurrentLevel() == 1)
        {
            StartCoroutine(FadeInLevelText(timeForLevelText));

            StartCoroutine(CloseStartGate(timeForStartGateClose));
        }
    }
    public void ChangeAttempsInCurrentLevel(int quantity)
    {
        numberOfAttemps[SceneManager.GetActiveScene().buildIndex - level1BuildIndex] += quantity;
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

        if((SceneManager.GetActiveScene().buildIndex) == level1BuildIndex)
        {
            levelText.text = "Tutorial";
        }
        else if ((SceneManager.GetActiveScene().buildIndex) == level1BuildIndex + numberOfLevels-1) //Si es el nivel 4
        {
            levelText.text = "King Kronos";
        }
        else
        {
            levelText.text = "Nave " + (SceneManager.GetActiveScene().buildIndex - level1BuildIndex);
        }
        levelText.gameObject.SetActive(true);

    }

    public void ActivateDeathPanel()
    {
        AudioManager.instance.PlaySFX(nameSFXdeathPanel);

        deathPanel.SetActive(true);
        CursorController.onGameplay = false;

        /*
        FindObjectOfType<ClockController>().levelTimeCanDecrease = false;
        FindObjectOfType<ClockController>().shipCanExplode = false;
        */
    }

    public void ActivateLevelCompletePanel()
    {
        if (!((SceneManager.GetActiveScene().buildIndex) == level1BuildIndex + numberOfLevels-1)) //Si no es el ultimo nivel (KingKronos)
        {
            AudioManager.instance.PlaySFX(nameSFXlevelCompletePanel);
        }
        else
        {
            StartCoroutine(AudioManager.instance.FadeOutGeneralVolume(0.35f));
        }
       
        levelCompletePanel.SetActive(true);

        /*
        Time.timeScale = 0f;
        PauseController.gamePaused = true;
        */
        
        CursorController.onGameplay = false;     
    }

    public void ActivateDenyNextLevelPanel()
    {
        AudioManager.instance.PlaySFX(nameSFXdenyLevelCompletePanel);
        denyNextLevelPanel.SetActive(true);
    }

    public IEnumerator NextLevelLogic()
    {
        if (!levelCompleted)
        {
            GameObject[] basicEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            int numberEnemiesRemaining = basicEnemies.Length;

            if (numberEnemiesRemaining <= 0)
            {
                PauseController.canPauseGame = false;
                levelCompleted = true;

                _clockController.levelTimeCanDecrease = false;
                _clockController.shipCanExplode = false;

                FindObjectOfType<MovementController>().Stop();
                FindObjectOfType<MovementController>().StopOnY();

                AudioManager.instance.PlaySFX(nameSFXgateOpen);
                endGate.GetComponent<Animator>().SetTrigger("Open");

                yield return new WaitForSeconds(timeToPopUpLevelCompletePanel);

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
                    yield return new WaitUntil(() => denyNextLevelPanelText.fontSize == 0); //Se espera que se haya cerrado si el tamaño del texto es 0;
                    denyNextLevelPanel.SetActive(false);
                }
            }
        }

    }

    public IEnumerator CloseStartGate(float time)
    {
        startGate.GetComponent<Animator>().SetTrigger("Open"); //La animacion solo cambia al sprite de puerta abierta

        yield return new WaitForSeconds(time);

        AudioManager.instance.PlaySFX(nameSFXgateClose);
        startGate.GetComponent<Animator>().SetTrigger("Close"); //Se cierra la puerta, estado abierto -> cerrado
    }
}
