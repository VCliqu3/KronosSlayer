using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryButtons : MonoBehaviour
{
    public Animator animator;
    
    //Ver los creditos

    public void  NextButton()
    {
        animator.Play("QuePasenLosCreditosXD");
    }

    public void OnCompletedQuePasenLosCreditosXD()
    {
        SceneManager.LoadScene("CreditsScene");
    }

}
