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

        fadeOutPanel.SetActive(true);
    }

    public void OnCompletedSiguienteLvlFade()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
}
