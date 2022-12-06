using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicController : MonoBehaviour
{
    public string nameMusicScene;
    private void Awake()
    {
        AudioManager.instance.generalVolumeFading = 1f;
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMusic(nameMusicScene);
    }
}
