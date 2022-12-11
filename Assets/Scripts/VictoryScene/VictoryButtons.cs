using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryButtons : MonoBehaviour
{
    public Animator animator;
    
    //Ver los creditos

    public void NextButton()
    {
        StartCoroutine(AudioManager.instance.FadeOutGeneralVolume(0.35f));

        animator.Play("QuePasenLosCreditosXD");
    }

    public void OnCompletedQuePasenLosCreditosXD()
    {
        ScoreController.ResetScoreInAllLevels();
        ScoreController.ResetEnemiesKilledInAllLevels();
        LevelController.ResetAttempsInAllLevels();

        SceneManager.LoadScene("CreditsScene");
    }

}
