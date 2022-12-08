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
        StartCoroutine(AudioManager.instance.FadeOutGeneralVolume(0.35f));

        animator.Play("FadeOut");
    }

    public void OnCompletedPlayFadeOut()
    {
        SceneManager.LoadScene("Level1");
    }

    //opciones
    
    public void  Opciones()
    {
        animator.Play("OpcionesFadeOut");
    }

    public void OnCompletedOpcionesFadeOut()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    //ControlsScene
    
    public void ControlsSceneButton()
    {
        animator.Play("ControlsSceneFadeOut");
    }

    public void OnCompletedControlsSceneFadeOut()
    {
        SceneManager.LoadScene("ControlsScene");
    }

    //Creditos
    
    public void CreditosButton()
    {
        StartCoroutine(AudioManager.instance.FadeOutGeneralVolume(0.35f));

        animator.Play("CreditosFadeOut");

    }

    public void OnCompletedCreditosFadeOut()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    //Salir

    public void Salir()
    {
        Application.Quit();
    }
}
