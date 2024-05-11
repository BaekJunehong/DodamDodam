using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using sceneData;
using mode;
using scenario;
using difficulty;

namespace gameCtrl{
public class ChangeGame : MonoBehaviour
{
    public event Action sceneChange;
    public int order = 0;
    Progress progress;

    // Start is called before the first frame update
    void Start()
    {
        progress = GameObject.Find("Progress").GetComponent<Progress>();
    }

    // Update is called once per frame
    void Update()
    {
        if(progress.prog >= 100) {
            switch (Mode.M)
            {
                case (modeType.play):
                    switch (SceneData.SC)
                    {
                        case sceneType.straight:
                            SceneData.SC = sceneType.curve;
                            sceneChange?.Invoke();
                            break;

                        case sceneType.curve:
                            SceneData.SC = sceneType.zigzag;
                            sceneChange?.Invoke();
                            break;

                        case sceneType.zigzag:
                            break;              
                    }
                    break;
                
                case (modeType.test):
                    order = order + 1;

                    if (order >= Scenario.TS.Count) {
                        Debug.Log("in");
                        SceneManager.LoadScene("Result");
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
}