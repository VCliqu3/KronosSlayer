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
        animator.Play("IrAlMenuFadeOut");
    }

    public void OnCompletedIrAlMenuFadeOutFadeOut()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
