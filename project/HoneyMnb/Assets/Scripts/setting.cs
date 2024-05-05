using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using difficulty;
using handSide;

public class Setting : MonoBehaviour
{
    public void onModeClicked()
    {
        string mode = gameObject.name;
        setMode(mode);
    }

    public void onDifficultyClicked()
    {
        switch (gameObject.name)
        {
            case "Easy":
                setDifficulty(difficulty.difficultyLevel.easy); 
                break;

            case "Normal":
                setDifficulty(difficulty.difficultyLevel.normal);
                break;

            case "Hard":
                setDifficulty(difficulty.difficultyLevel.hard);
                break;
        }
    }

    public void setMode(string mode)
    {
        Debug.Log(mode);
    }

    public void setDifficulty(difficulty.difficultyLevel dif)
    {
        Difficulty.DF = dif;
        Debug.Log(Difficulty.DF);
    }
}
