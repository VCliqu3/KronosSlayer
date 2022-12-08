using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsButtons : MonoBehaviour
{
    public Animator animator;
    
    //Ir al menu

    public void  IrAlMenu()
    {
        StartCoroutine(AudioManager.instance.FadeOutGeneralVolume(0.35f));

        animator.Play("IrAlMenuFadeOut");
    }

    public void OnCompletedIrAlMenuFadeOutFadeOut()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
