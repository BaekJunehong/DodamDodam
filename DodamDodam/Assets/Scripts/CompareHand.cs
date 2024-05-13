using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandUtils;

public class CompareHand : MonoBehaviour
{
    HandTracker _handtracker;
    private Vector3 handTop;
    private Vector3 handBot;
    private Vector3 screenTop;
    private Vector3 screenBot;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        _handtracker = gameObject.AddComponent<HandTracker>();
        //screen 내의 hand 좌표 초기화
    }

    // Update is called once per frame
    void Update()
    {
        if(checkUp()){
            //성공!
        }
    }

    private bool checkUp(){
        if(compareHand()){
            time += Time.deltaTime;
            if(time >= 3f){
                time = 0;
                return true;
            }
        }
        time = 0;
        return false;
    }
    private bool compareHand(){
        handTop = _handtracker.GetVertex(12);
        handBot = _handtracker.GetVertex(0);

        return 0.1f >= Vector3.Distance(handTop, screenTop) &&  0.1f >= Vector3.Distance(handBot, screenBot);
    }
}
