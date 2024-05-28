using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandUtils;
using Microsoft.Unity.VisualStudio.Editor;
//using System.Numerics;
using UnityEngine.SceneManagement;

public class CompareHand : MonoBehaviour
{
    HandTracker _handtracker;
    private Vector3 handTop;
    private Vector3 handBot;
    private Vector3 screenTop;
    private Vector3 screenBot;
    float time = 0;
    public GameObject hand;
    RectTransform hand_transform;

    // Start is called before the first frame update
    void Start()
    {
        hand_transform = hand.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        hand_transform.GetWorldCorners(corners);
        screenTop = new Vector3((corners[1].x + corners[2].x) / 2f, corners[1].y, 0);
        screenBot = new Vector3((corners[1].x + corners[2].x) / 2f, corners[0].y, 0);
        Debug.Log(screenTop);
        _handtracker = gameObject.AddComponent<HandTracker>();
        //screen 내의 hand 좌표 초기화
    }

    // Update is called once per frame
    void Update()
    {
        if(checkUp()){
            SceneManager.LoadScene("GamePlay");
        }
    }

    private bool checkUp(){
        if(compareHand()){
            time += Time.deltaTime;
            Debug.Log(time);
            if(time >= 3f){
                time = 0;
                return true;
            }
        }
        //time = 0;
        return false;
    }
    private bool compareHand(){
        handTop = _handtracker.GetVertex(12);
        handBot = _handtracker.GetVertex(0);


        return 4f >= Vector3.Distance(handTop, screenTop) &&  4f >= Vector3.Distance(handBot, screenBot);
    }
}
