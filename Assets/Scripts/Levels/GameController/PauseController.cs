using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    static public bool gamePaused;
    static public bool canPauseGame;

    public GameObject pausePanel;
    public GameObject volumePanel;
    private Animator _pausePanelAnimator;
    private Image _pausePanelImage;
    // Start is called before the first frame update
    void Start()
    {
        gamePaused = false;
        canPauseGame = true;

        _pausePanelAnimator = pausePanel.GetComponent<Animator>();
        _pausePanelImage = pausePanel.GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused && canPauseGame)
            {
                Pause();
            }
            else if (pausePanel.activeInHierarchy && !volumePanel.activeInHierarchy)
            {
                StartCoroutine(Resume());
            }
        }
    }

    public void Pause()
    {      
        pausePanel.SetActive(true);
        _pausePanelAnimator.SetTrigger("Pause");

        Time.timeScale = 0f;
        gamePaused = true;       
    }

    public IEnumerator Resume()
    {        
        _pausePanelAnimator.SetTrigger("Resume");
        yield return new WaitUntil(() => _pausePanelImage.color.a == 0); //Se espera que sea completamente transparente
        pausePanel.SetActive(false);

        Time.timeScale = 1f;
        gamePaused = false;

    }

    public void CallResume()
    {
        StartCoroutine(Resume());
    }
}
