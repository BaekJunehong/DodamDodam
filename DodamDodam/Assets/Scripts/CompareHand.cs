using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HandUtils;
using Microsoft.Unity.VisualStudio.Editor;
//using System.Numerics;
using UnityEngine.SceneManagement;

public class CompareHand : MonoBehaviour
{
    HandTracker _handtracker;
    private Vector3 handTop;
    private Vector3 handBot;
    private Vector3 screenTopMax;
    private Vector3 screenTopMin;
    private Vector3 screenBotMax;
    private Vector3 screenBotMin;
    private Vector3 worldTopMax;
    private Vector3 worldTopMin;
    private Vector3 worldBotMax;
    private Vector3 worldBotMin;
    private string[] directions = new string[] { "유지", "아래로", "위로", "가까히", "멀리" };
    private int totalSections = 6;
    public bool is_done = false;
    public RectTransform border;
    public Text direction;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        float imageHeight = border.rect.height;
        float sectionHeight = imageHeight / totalSections;

        screenTopMax = new Vector3(0, imageHeight - sectionHeight);
        screenTopMin = new Vector3(0, imageHeight - 2*sectionHeight);
        screenBotMax = new Vector3(0, imageHeight - 4*sectionHeight);
        screenBotMin = new Vector3(0, imageHeight - 5*sectionHeight);

        worldTopMax = border.TransformPoint(screenTopMax);
        worldTopMin = border.TransformPoint(screenTopMin);
        worldBotMax = border.TransformPoint(screenBotMax);
        worldBotMin = border.TransformPoint(screenBotMin);

        _handtracker = gameObject.AddComponent<HandTracker>();
        //screen 내의 hand 좌표 초기화
    }

    // Update is called once per frame
    void Update()
    {
        if(is_done){
            Debug.Log(time);
            if(checkUp()){
                SceneManager.LoadScene("GamePlay");
            }
        }
    }

    private bool checkUp(){
        int check = compareHand();

        direction.text = directions[check];
        if(check == 0){
            time += Time.deltaTime;
            
            if(time >= 3f){
                time = 0;
                return true;
            }
        }
        //time = 0;
        return false;
    }
    private int compareHand(){
        handTop = _handtracker.GetVertex(12);
        handBot = _handtracker.GetVertex(0);

        // 손 윗 부분이 넘어서는 경우
        if (handTop.y > worldTopMax.y)
        {
            // 손 아랫 부분이 넘어서는 경우
            if (handBot.y > worldBotMax.y)
            {
                return 1;
            }
            // 손 아랫 부분이 적절한 경우
            else if (handBot.y > worldBotMin.y)
            {
                return 4;
            }
            // 손 아랫 부분이 모자라는 경우
            else
            {
                return 4;
            }
        }
        // 손 윗 부분이 적절할 경우
        else if (handTop.y > worldTopMin.y)
        {
            // 손 아랫 부분이 넘어서는 경우
            if (handBot.y > worldBotMax.y)
            {
                return 3;
            }
            // 손 아랫 부분이 적절한 경우
            else if (handBot.y > worldBotMin.y)
            {
                return 0;
            }
            // 손 아랫 부분이 모자라는 경우
            else
            {
                return 4;
            }
        }
        // 손 윗 부분이 모자라는 경우
        else
        {
            // 손 아랫 부분이 넘어서는 경우
            if (handBot.y > worldBotMax.y)
            {
                return 3;
            }
            // 손 아랫 부분이 적절한 경우
            else if (handBot.y > worldBotMin.y)
            {
                return 3;
            }
            // 손 아랫 부분이 모자라는 경우
            else
            {
                return 2;
            }
        }
    }
}
