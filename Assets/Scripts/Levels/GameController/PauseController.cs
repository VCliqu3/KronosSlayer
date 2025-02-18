using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    static public bool gamePaused;
    static public bool canPauseGame;

    public Animator animator;

    public GameObject pausePanel;
    public GameObject volumePanel;
    private Animator _pausePanelAnimator;
    private Image _pausePanelImage;

    public bool setStartPause = false;

    //SFX

    public string nameSFXpauseOpen;
    public string nameSFXpauseClose;

    // Start is called before the first frame update
    void Start()
    {
        if (setStartPause)
        {
            gamePaused = false;
        }

        canPauseGame = true;

        _pausePanelAnimator = pausePanel.GetComponent<Animator>();
        _pausePanelImage = pausePanel.GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused && canPauseGame)
            {
                gamePaused = true;
                Pause();
            }
            else if (pausePanel.activeInHierarchy && !volumePanel.activeInHierarchy && gamePaused)
            {
                StartCoroutine(Resume());
            }
        }
    }

    public void Pause()
    {
        gamePaused = true;
        AudioManager.instance.PlaySFX(nameSFXpauseOpen);

        pausePanel.SetActive(true);
        _pausePanelAnimator.SetTrigger("Pause");

        Time.timeScale = 0f;

        CursorController.onGameplay = false;
    }

    public IEnumerator Resume()
    {
        AudioManager.instance.PlaySFX(nameSFXpauseClose);

        _pausePanelAnimator.SetTrigger("Resume");
        yield return new WaitUntil(() => _pausePanelImage.color.a == 0); //Se espera que sea completamente transparente
        pausePanel.SetActive(false);

        Time.timeScale = 1f;
        gamePaused = false;

        CursorController.onGameplay = true;
    }

    public void CallResume()
    {
        StartCoroutine(Resume());
    }

    //Para reintentar//
    public void ReintentarBoton()
    {
        animator.Play("ReintentarAni");
    }

    public void OnCompletedReintentarAni()
    {
        Time.timeScale = 1f;

        FindObjectOfType<ScoreController>().ResetScoreInCurrentLevel();
        FindObjectOfType<ScoreController>().ResetEnemiesKilledInCurrentLevel();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //Para reintentar//

    //PA IR AL MENU XD//

    public void MainMenuBoton()
    {
        StartCoroutine(AudioManager.instance.FadeOutGeneralVolume(0.25f));
        animator.Play("MainMenuAni");
    }

    public void OnCompletedMainMenuAni()
    {
        Time.timeScale = 1f;
       
        ScoreController.ResetScoreInAllLevels();
        ScoreController.ResetEnemiesKilledInAllLevels();
        LevelController.ResetAttempsInAllLevels();
        
        SceneManager.LoadScene("MainMenu");
    }

    //PA IR AL MENU XD//
}
