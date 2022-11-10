using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public float timeToCompleteLevel;
    public float levelTime;
    public int rawlevelTime;
    private int previousRawTime;

    public bool timeHasChanged = false;

    public bool levelTimeCanDecrease = true;

    private HUDController _HUDController;
    private LevelController _levelController;

    // Start is called before the first frame update
    void Start()
    {
        _HUDController = FindObjectOfType<HUDController>();
        _levelController = GetComponent<LevelController>();
        levelTime = timeToCompleteLevel;
        previousRawTime = rawlevelTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (levelTimeCanDecrease && levelTime>=0 && !_levelController.levelCompleted)
        {
            levelTime -= Time.deltaTime;
        }

        rawlevelTime = Mathf.CeilToInt(levelTime);

        CheckIfTimeChanged();
    }

    public void CheckIfTimeChanged()
    {
        if(rawlevelTime != previousRawTime)
        {
            timeHasChanged = true;
            previousRawTime = rawlevelTime;
            _HUDController.SetTimeText();

            if(rawlevelTime <= 0)
            {
                //ShipAutodestruction()
            }
        }
        else
        {
            timeHasChanged = false;
        }
    }

    void ShipAutodestruction()
    {

    }
}
