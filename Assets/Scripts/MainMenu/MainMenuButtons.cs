using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public Animator animator;

    public void Play()
    {
        animator.Play("FadeOut");
        //SceneManager.LoadScene("SampleScene");//
    }

    public void OnCompletedFadeOut()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
