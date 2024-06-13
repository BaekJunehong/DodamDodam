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
    public GameObject timer;
    
    // Start is called before the first frame update
    void Start()
    {
        Timer.T = 0.00f;
        ErrorCount.E = 0;
    }

    // Update is called once per frame
    void Update()
    {
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

    public void nextGame()
    {
        order = order + 1;
        switch (Mode.M)
        {
            case (modeType.play):
                if (order >= Scenario.PS.Count) {
                    GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("GameComponent");

                    result_canvas.SetActive(true);
                    //score.text = GameObject.Find("Timer").GetComponent<Timer>().T.ToString("N2");
                    //error.text = GameObject.Find("Error").GetComponent<ErrorCount>().E.ToString("N");
                    score.text = Timer.T.ToString("N2");
                    error.text = ErrorCount.E.ToString("N4");

                    foreach (GameObject obj in objectsWithTag)
                    {
                        Destroy(obj);
                    }

                    // result_canvas.SetActive(true);
                    // score.text = timer.GetComponent<Timer>().T.ToString("N2");
                    // error.text = ErrorCount.errorCount.ToString();
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