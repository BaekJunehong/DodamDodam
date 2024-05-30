using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HandUtils;
using Microsoft.Unity.VisualStudio.Editor;
//using System.Numerics;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class CompareHand : MonoBehaviour
{
    HandTracker _handtracker;
    private Vector3 handTop;
    private Vector3 handBot;
    private Vector3 screenHandTop;
    private Vector3 screenHandBot;
    private Vector3 uiTopMax;
    private Vector3 uiTopMin;
    private Vector3 uiBotMax;
    private Vector3 uiBotMin;
    private Vector3 worldTopMax;
    private Vector3 worldTopMin;
    private Vector3 worldBotMax;
    private Vector3 worldBotMin;
    private Vector3 screenTopMax;
    private Vector3 screenTopMin;
    private Vector3 screenBotMax;
    private Vector3 screenBotMin;
    private string[] directions = new string[] { "유지", "아래로", "위로", "가까히", "멀리" };
    public Camera screen_cam;
    public Camera hand_cam;
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
        Vector3 localPosition = border.localPosition;
        Vector2 size = border.rect.size;

        uiTopMax = new Vector3(localPosition.x, (localPosition.y+((size.y)/2f))-(size.y/6f), localPosition.z);
        uiTopMin = new Vector3(localPosition.x, (localPosition.y+((size.y)/2f))-2*(size.y/6f), localPosition.z);
        uiBotMax = new Vector3(localPosition.x, (localPosition.y+((size.y)/2f))-4*(size.y/6f), localPosition.z);
        uiBotMin = new Vector3(localPosition.x, (localPosition.y+((size.y)/2f))-5*(size.y/6f), localPosition.z);

        worldTopMax = border.TransformPoint(uiTopMax);
        worldTopMin = border.TransformPoint(uiTopMin);
        worldBotMax = border.TransformPoint(uiBotMax);
        worldBotMin = border.TransformPoint(uiBotMin);

        screenTopMax = screen_cam.WorldToScreenPoint(worldTopMax);
        screenTopMin = screen_cam.WorldToScreenPoint(worldTopMin);
        screenBotMax = screen_cam.WorldToScreenPoint(worldBotMax);
        screenBotMin = screen_cam.WorldToScreenPoint(worldBotMin);

        Debug.Log(screenTopMax);
        Debug.Log(screenTopMin);

        _handtracker = gameObject.AddComponent<HandTracker>();
        //screen 내의 hand 좌표 초기화
    }

    // Update is called once per frame
    void Update()
    {
        if(is_done){
            if(checkUp()){
                SceneManager.LoadScene("GamePlay");
            }
        }
    }

    private bool checkUp(){
        int check = compareHand();
        direction.text = directions[check];
        Debug.Log(direction.text);
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
        handTop = _handtracker.GetRawVertex(12);
        handBot = _handtracker.GetRawVertex(0);

        //Vector3 worldHandTop = hand_cam.ScreenToWorldPoint(handTop);
        screenHandTop = screen_cam.WorldToScreenPoint(handTop);
        screenHandBot = screen_cam.WorldToScreenPoint(handBot);


        // 손 윗 부분이 넘어서는 경우
        if (screenHandTop.y > screenTopMax.y)
        {
            // 손 아랫 부분이 넘어서는 경우
            if (screenHandBot.y > screenBotMax.y)
            {
                return 1;
            }
            // 손 아랫 부분이 적절한 경우
            else if (screenHandBot.y > screenBotMin.y)
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
        else if (screenHandTop.y > screenTopMin.y)
        {
            // 손 아랫 부분이 넘어서는 경우
            if (screenHandBot.y > screenBotMax.y)
            {
                return 3;
            }
            // 손 아랫 부분이 적절한 경우
            else if (screenHandBot.y > screenBotMin.y)
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
            if (screenHandBot.y > screenBotMax.y)
            {
                return 3;
            }
            // 손 아랫 부분이 적절한 경우
            else if (screenHandBot.y > screenBotMin.y)
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
