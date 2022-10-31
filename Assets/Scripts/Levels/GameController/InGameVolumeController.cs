using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameVolumeController : MonoBehaviour
{
    public GameObject volumePanel;
    private Animator _volumePanelAnimator;
    private Image _volumePanelImage;

    // Start is called before the first frame update
    void Start()
    {
        _volumePanelAnimator = volumePanel.GetComponent<Animator>();
        _volumePanelImage = volumePanel.GetComponent<Image>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            if (!PauseController.gamePaused && PauseController.canPauseGame)
            {
                VolumePause();
            }
            else if (volumePanel.activeInHierarchy)
            {
                StartCoroutine(VolumeResume());
            }
        }
    }

    public void VolumePause()
    {
        volumePanel.SetActive(true);
        _volumePanelAnimator.SetTrigger("Pause");

        Time.timeScale = 0f;
        PauseController.gamePaused = true;
    }
    public IEnumerator VolumeResume()
    {
        _volumePanelAnimator.SetTrigger("Resume");
        yield return new WaitUntil(() => _volumePanelImage.color.a == 0); //Se espera que sea completamente transparente
        volumePanel.SetActive(false);

        Time.timeScale = 1f;
        PauseController.gamePaused = false;

    }
    public void CallVolumeResume()
    {
        StartCoroutine(VolumeResume());
    }
}
