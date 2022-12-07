using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsLevelComplete : MonoBehaviour
{
    public Animator animator;
    public GameObject fadeOutPanel;

    public void SiguienteLvl()
    {
        //Time.timeScale = 1f;
        //animator.Play("SiguienteLvlFade");

        int currentLvlIndex = SceneManager.GetActiveScene().buildIndex;

        if ((currentLvlIndex == LevelController.level1BuildIndex+2)|| (currentLvlIndex == LevelController.level1BuildIndex + 3)) //Si es el nivel 3 o 4(Boss)
        {
            StartCoroutine(AudioManager.instance.FadeOutGeneralVolume(0.25f));
        }
        
        fadeOutPanel.SetActive(true);
    }

    public void OnCompletedSiguienteLvlFade()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
}
