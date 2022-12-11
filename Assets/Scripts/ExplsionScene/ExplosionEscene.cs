using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExplosionEscene : MonoBehaviour
{
    public Animator animator;

    //SFX

    public string nameSFXbombExplosion;

    void Start()
    {
        AudioManager.instance.PlaySFX(nameSFXbombExplosion);
    }

    //-------ReintentarLvl1-------//

    public void ReintentarButonlvl1()
    {
        StartCoroutine(AudioManager.instance.FadeOutGeneralVolume(0.25f));
        animator.Play("ReintentarAnilvl1");
    }

    public void OnCompletedReintentarAnilvl1()
    {
        ScoreController.score[LevelController.currentLevelIndex - LevelController.level1BuildIndex] = 0;
        ScoreController.enemiesKilled[LevelController.currentLevelIndex - LevelController.level1BuildIndex] = 0;

        SceneManager.LoadScene(LevelController.currentLevelIndex);
    }

    //-------------PARA IR AL MENU--------------//
    
    public void MainMenuButon()
    {
        StartCoroutine(AudioManager.instance.FadeOutGeneralVolume(0.25f));
        animator.Play("MainMenuAnim");
    }

    public void OnCompletedMainMenuAni()
    {
        ScoreController.ResetScoreInAllLevels();
        ScoreController.ResetEnemiesKilledInAllLevels();
        LevelController.ResetAttempsInAllLevels();
        
        SceneManager.LoadScene("MainMenu");
    }
}
