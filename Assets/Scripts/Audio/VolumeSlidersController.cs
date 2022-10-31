using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlidersController : MonoBehaviour
{
    public Slider generalSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("GeneralVolume"))
        {
            PlayerPrefs.SetFloat("GeneralVolume", 1);
        }
        else
        {
            LoadGeneralVolume();
        }

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
        }
        else
        {
            LoadMusicVolume();
        }

        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 1);
        }
        else
        {
            LoadSFXVolume();
        }
    }
    public void LoadGeneralVolume()
    {
        generalSlider.value = PlayerPrefs.GetFloat("GeneralVolume");
    }
    public void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }
    public void LoadSFXVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void ChangeGeneralVolume()
    {
        AudioManager.instance.generalVolume = generalSlider.value;
        PlayerPrefs.SetFloat("GeneralVolume", AudioManager.instance.generalVolume);
        AudioManager.instance.SetGeneralVolume();
    }

    public void ChangeMusicVolume()
    {
        AudioManager.instance.musicVolume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", AudioManager.instance.musicVolume);
        AudioManager.instance.SetMusicVolume();
    }

    public void ChangeSFXVolume()
    {
        AudioManager.instance.sfxVolume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", AudioManager.instance.sfxVolume);
        AudioManager.instance.SetSFXVolume();
    }

}
