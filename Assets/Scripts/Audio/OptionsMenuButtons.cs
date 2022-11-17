using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuButtons : MonoBehaviour
{
    public Animator animator;

    public void IrAlMenu()
    {
        animator.Play("IrAlMenuFadeOut");
    }

    public void OnCompletedIrAlMenuFadeOut()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
