using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorController : MonoBehaviour
{
    static public bool onGameplay;

    public Texture2D RegularCursor;
    public Texture2D RangedCursor;
    public Texture2D MeleeCursor;

    public string currentState = "NotOnGameplay";
    public string previousState = "NotOnGameplay";
    // Start is called before the first frame update
    void Start()
    {
        onGameplay = false;

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        for (int i=LevelController.level1BuildIndex; i< (LevelController.level1BuildIndex + LevelController.numberOfLevels); i++)
        {
            if (sceneIndex == i)
            {
                onGameplay = true;
            }
        }

        CheckState();

        SetCursor();
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();

        if (CheckIfStateHasChanged())
        {
            SetCursor();
        }
    }

    void CheckState()
    {
        if (!onGameplay || PauseButonController.mouseOnPauseButton) //|| PauseButonController.mouseOnPauseButton
        {
            currentState = "NotOnGameplay";
        }
        else
        {
            if (ModeController.isRanged)
            {
                currentState = "Gameplay_Ranged";
            }
            else
            {
                currentState = "Gameplay_Melee";
            }
        }
    }

    bool CheckIfStateHasChanged()
    {
        bool hasChanged = false;

        if(currentState != previousState)
        {
            hasChanged = true;
            previousState = currentState;
        }

        return hasChanged;
    }

    void SetCursor()
    {
        if(currentState == "NotOnGameplay")
        {
            Cursor.SetCursor(RegularCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            if (ModeController.isRanged)
            {
                Cursor.SetCursor(RangedCursor, new Vector2(RangedCursor.width/2,RangedCursor.height/2), CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(MeleeCursor, Vector2.zero, CursorMode.Auto);
            }
        }
    }


}
