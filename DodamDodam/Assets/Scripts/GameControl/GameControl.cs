using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using sceneData;
using mode;
using scenario;
using difficulty;

namespace gameCtrl{
public class ChangeGame : MonoBehaviour
{
    public event Action sceneChange;
    public int order = 0;
    public GameObject result_canvas;
    public TextMeshProUGUI score;
    public TextMeshProUGUI error;
    Progress progress;
    public GameObject timer;
    
    // Start is called before the first frame update
    void Start()
    {
        progress = GameObject.Find("Progress").GetComponent<Progress>();
    }

    // Update is called once per frame
    void Update()
    {
        if(progress.prog >= 100) {
            // order = order + 1;

            // switch (Mode.M)
            // {
            //     case (modeType.play):
            //         if (order >= Scenario.PS.Count) {
                
            //         }

            //         else {
            //             SceneData.SC = Scenario.PS[order];
            //             sceneChange?.Invoke();
            //         }
            //         break;
                
            //     case (modeType.test):
                    

            //         if (order >= Scenario.TS.Count) {
            //             SceneManager.LoadScene("Result");
            //         }

            //         else {
            //             Difficulty.DF = Scenario.TS[order].Item1;
            //             SceneData.SC = Scenario.TS[order].Item2;

            //             sceneChange?.Invoke();
            //         }
            //         break;
            // }
        }
    }

    public void nextGame()
    {
        order = order + 1;
        switch (Mode.M)
        {
            case (modeType.play):
                if (order >= Scenario.PS.Count) {
                    result_canvas.SetActive(true);
                    print("끝");

                    score.text = timer.GetComponent<Timer>().T.ToString("N2");
                    error.text = ErrorCount.errorCount.ToString();
                }

                else {
                    SceneData.SC = Scenario.PS[order];
                    sceneChange?.Invoke();
                }
                break;
                
            case (modeType.test):
                if (order >= Scenario.TS.Count) {
        //            SceneManager.LoadScene("Result");
                    SceneManager.LoadScene("Loading");
                }

                else {
                    Difficulty.DF = Scenario.TS[order].Item1;
                    SceneData.SC = Scenario.TS[order].Item2;

                    sceneChange?.Invoke();
                }
                break;
        }
    }
}
}