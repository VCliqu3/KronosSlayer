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
    }
}
