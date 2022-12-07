using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicController : MonoBehaviour
{
    public string nameMusicScene;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.generalVolumeFading = 1f;
        AudioManager.instance.PlayMusic(nameMusicScene);

        InitialSet();
    }

    void InitialSet()
    {
        if (!PlayerPrefs.HasKey("GeneralVolume"))
        {
            PlayerPrefs.SetFloat("GeneralVolume", 1);
        }

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
        }

        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 1);
        }

        AudioManager.instance.generalVolume = PlayerPrefs.GetFloat("GeneralVolume");
        AudioManager.instance.SetGeneralVolume();

        AudioManager.instance.musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        AudioManager.instance.SetMusicVolume();

        AudioManager.instance.sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        AudioManager.instance.SetSFXVolume();
    }
}
