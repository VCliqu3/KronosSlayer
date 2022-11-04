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
        if (Input.GetKeyDown(KeyCode.Escape) && volumePanel.activeInHierarchy)
        {
            CloseVolumePanel();
        }
    }

    public void ActivateVolumePanel()
    {
        volumePanel.SetActive(true);
    }
    public void CloseVolumePanel()
    {
        volumePanel.SetActive(false);

    }
}
