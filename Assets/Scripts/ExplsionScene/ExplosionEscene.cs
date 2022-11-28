using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExplosionEscene : MonoBehaviour
{
    public Animator animator;

    //-------ReintentarLvl1-------//

    public void ReintentarButonlvl1()
    {
        animator.Play("ReintentarAnilvl1");
    }

    public void OnCompletedReintentarAnilvl1()
    { 
        SceneManager.LoadScene("Level1");
    }

    //-------------PARA IR AL MENU--------------//
    
    public void MainMenuButon()
    {
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
