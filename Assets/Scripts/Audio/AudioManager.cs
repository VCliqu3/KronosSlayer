using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource, perpetualSFXSource;

    public float generalVolume;
    public float musicVolume;
    public float sfxVolume;

    public float generalVolumeFading = 1;

    public float timeToFadeGeneral;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound m = Array.Find(musicSounds, x => x.clip == musicSource.clip); //m es la musica que se esta reproduciendo, puede ser null

        string currentClipName;

        if (m == null)
        {
            currentClipName = "NoClipPlaying";
        }
        else
        {
            currentClipName = m.name;
        }

        if (name != currentClipName)
        {
            //
            Sound s = Array.Find(musicSounds, x => x.name == name);

            if (s == null)
            {
                Debug.Log("Sound Not Found");
                Debug.Log(name);
            }
            else
            {
                musicSource.clip = s.clip;
                musicSource.Play();
            }
            //
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
            Debug.Log(name);
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlayPerpetualSFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
            Debug.Log(name);
        }
        else
        {
            perpetualSFXSource.clip = s.clip;
            perpetualSFXSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void StopPerpetualSFX()
    {
        perpetualSFXSource.Stop();
    }

    public void SetGeneralVolume()
    {
        musicSource.volume = musicVolume  * generalVolume * generalVolumeFading ;
        sfxSource.volume = sfxVolume  * generalVolume * generalVolumeFading ;
        perpetualSFXSource.volume = sfxVolume  * generalVolume * generalVolumeFading ;
    }
    public void SetMusicVolume()
    {
        musicSource.volume = musicVolume*generalVolume*generalVolumeFading;
    }

    public void SetSFXVolume()
    {
        sfxSource.volume = sfxVolume   * generalVolume * generalVolumeFading ;
        perpetualSFXSource.volume = sfxVolume  * generalVolume * generalVolumeFading ;
    }

    public IEnumerator FadeOutGeneralVolume(float timeToFade)
    {
        while(generalVolumeFading > 0)
        {
            generalVolumeFading -= 0.01f;
            SetGeneralVolume();
            yield return new WaitForSeconds(timeToFade * 0.01f);
        }
    }

    public IEnumerator FadeInGeneralVolume(float timeToFade)
    {
        while (generalVolumeFading <= 1)
        {
            generalVolumeFading += 0.01f;
            SetGeneralVolume();
            yield return new WaitForSeconds(timeToFade * 0.01f);
        }
    }
}
