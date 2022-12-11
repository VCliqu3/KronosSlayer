using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathPanel : MonoBehaviour
{
    public Animator animator;

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
        StartCoroutine(AudioManager.instance.FadeOutGeneralVolume(0.15f));
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
