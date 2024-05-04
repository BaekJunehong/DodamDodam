using System;
using UnityEngine;
using sceneData;

namespace gameCtrl{
public class ChangeGame : MonoBehaviour
{
    public event Action sceneChange;
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
        }
    }
}
}