using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using difficulty;
using handSide;
using mode;
using scenario;
using sceneData;

public class Setting : MonoBehaviour
{
    public GameObject mask;
    public GameObject handSelect;
    public GameObject modeSelect;
    public GameObject difficultySelect;
    public void onPlayClicked()
    {
        setMode(mode.modeType.play);
        modeOff();
        difficultyOn();
    }

    public void onTestClicked()
    {
        setMode(mode.modeType.test);
        setDifficulty(Scenario.TS[0].Item1);
        setLineType(Scenario.TS[0].Item2);
        modeOff();
        handOn();  
    }

    public void onEasyClicked()
    {
        setDifficulty(difficulty.difficultyLevel.easy); 
        difficultyOff();
        handOn();
    }

    public void onNormalClicked()
    {
        setDifficulty(difficulty.difficultyLevel.normal); 
        difficultyOff();
        handOn();
    }

    public void onHardClicked()
    {
        setDifficulty(difficulty.difficultyLevel.hard); 
        difficultyOff();
        handOn();
    }

    public void onLeftClicked()
    {
        setHand(handSide.whichSide.left);
        handOff();
        maskOff();
    }

    public void onRightClicked()
    {
        setHand(handSide.whichSide.right);
        handOff();
        maskOff();
    }

    public void setMode(mode.modeType m)
    {
        Mode.M = m;
    }

    public void setDifficulty(difficulty.difficultyLevel dif)
    {
        Difficulty.DF = dif;
    }

    public void setHand(handSide.whichSide hand)
    {
        HandSide.HS = hand;
    }

    public void setLineType(sceneData.sceneType line)
    {
        SceneData.SC = line;
    }

    public void maskOn()
    {
        mask.SetActive(true);
    }

    public void maskOff()
    {
        mask.SetActive(false);
    }
    public void modeOn()
    {
        modeSelect.SetActive(true);
    }
    public void modeOff()
    {
        modeSelect.SetActive(false);
    }

    public void difficultyOn()
    {
        difficultySelect.SetActive(true);
    }

    public void difficultyOff()
    {
        difficultySelect.SetActive(false);
    }

    public void handOn()
    {
        handSelect.SetActive(true);
    }

    public void handOff()
    {
        handSelect.SetActive(false);
    }
}
