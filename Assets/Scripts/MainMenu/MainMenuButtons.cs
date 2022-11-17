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
    }

    public void OnCompletedPlayFadeOut()
    {
        SceneManager.LoadScene("Level1");
    }

    //opciones//
    
    public void  Opciones()
    {
        animator.Play("OpcionesFadeOut");
    }

    public void OnCompletedOpcionesFadeOut()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    //opciones//


    public void Salir()
    {
        Application.Quit();
    }
}
