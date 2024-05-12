using System;
using UnityEngine;
using HandUtils;
using difficulty;
using handSide;
using sceneData;
using gameCtrl;

public class CuttedPoint : MonoBehaviour
{
    public event Action Error;
    public float Width = 0.25f;
    public GameObject objectHand;
    public GameObject scissors;
    public GameObject objectOfLine;
    public GameObject objectOfRedLine;
    public GameObject ctrl;
    
    private HandTracker _handtracker;
    private LineRenderer lineRenderer;
    private LineRenderer dottedLine;
    private LineRenderer redLine;
    private hand Hand;
    private float distance;
    private Vector3 closestPoint;
    private RectTransform paperRT;
    private DottedLine dottedData;
    private ChangeGame gCtrl;


    void Start()
    {
        paperRT = GameObject.Find("paper").GetComponent<RectTransform>();
        dottedLine = objectOfLine.GetComponent<LineRenderer>();
        dottedData = objectOfLine.GetComponent<DottedLine>();
        _handtracker = gameObject.AddComponent<HandTracker>();
        redLine = objectOfRedLine.GetComponent<LineRenderer>();
        gCtrl = ctrl.GetComponent<ChangeGame>();
        
        

        Hand = FindObjectOfType<hand>();
        if(Hand != null){
            Hand.isHold += (isGrabbed)=> {
                if(isGrabbed){
                    DrawPath();
                    if(isFinish()){
                        gCtrl.nextGame();
                    }
                }
            };
        }
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 1; // 시작 위치를 라인에 추가
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.startWidth = Width;
        lineRenderer.endWidth = Width;

        redLine.startWidth = Width;
        redLine.endWidth = Width;
    }
    
    void DrawPath()
    {
        if(Vector3.Distance(scissors.transform.position, transform.position) <= 0.7f && Vector3.Distance(scissors.transform.position, objectHand.transform.position) <= 0.5f){//점과 가위의 거리가 가까울 때 발생한다.
            Vector3 direction = _handtracker.GetDirection();
            int power = _handtracker.Cutting();
            if(power >= 1){
                Vector3 destination = power >= 1 ? transform.position + direction * power : Vector3.zero;//목표점 계산
            
                int pointsCount = dottedLine.positionCount;
                Vector3[] points = new Vector3[pointsCount];
                dottedLine.GetPositions(dottedData.data.points);//현재 위치한 씬의 점선 불러오기
                //경계를 벗어났을 때에 대한 예외 처리
                bool flag = true;
                bool excapebit = false;
                for(int i=1; i<=5; i++){
                    float ratio = (float)i / 5;
                    Vector3 divisionPoint = Vector3.Lerp(transform.position, destination, ratio);
                    findClosestPointAndDistance(points, divisionPoint);
                    float difficulty = SceneData.SC != sceneType.zigzag ? (float)Difficulty.DF : Difficulty.DF == difficultyLevel.easy? 2f : Difficulty.DF == difficultyLevel.normal? 1.5f : 1f;
                    if(distance >= difficulty){
                        flag = false;
                    }
                    if ((HandSide.HS == whichSide.right && divisionPoint.x <= -(paperRT.rect.width / 2f)*0.9f) || (HandSide.HS == whichSide.left && divisionPoint.x >= (paperRT.rect.width / 2f)*0.9f)) {excapebit = true;}
                }
                if(!flag && !excapebit){
                    redLine.positionCount = 2;
                    redLine.SetPosition(0, transform.position);
                    redLine.SetPosition(1, destination);
                    redLine.startWidth = Width;
                    redLine.endWidth = Width;
                    Invoke("DestroyRedLine", 0.5f);
                }
                //점선과의 거리가 난이도에 해당하는 굵기보다 작을 때만 그리기
                else{
                    lineRenderer.positionCount ++; // 점의 수를 증가시킴
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, destination); // 새로운 위치를 추가
                    transform.position = destination;

                    
                }
            }
        }
    }

    private void DestroyRedLine()
    {
        // 라인 렌더러 제거
        redLine.positionCount = 0;
        Error?.Invoke();
    }

    //배열 내 모든 선분 위의 최근접 점을 찾는 함수
    public void findClosestPointAndDistance(Vector3[] points, Vector3 position){
        distance = float.MaxValue;
        for (int i = 0; i < points.Length-1; i++){
            Vector3 start = points[i];
            Vector3 end = points[i+1];
            Vector3 closestpoints = ClosestPointOnLineSegment(start, end, position);
            float currentDistance = Vector3.Distance(position, closestpoints);
            if(currentDistance < distance){
                distance = currentDistance;
                closestPoint = closestpoints;
            }
        }  
    }
    //한 선분 위의 최근접 포인트를 찾는 함수
    public Vector3 ClosestPointOnLineSegment(Vector3 A, Vector3 B, Vector3 P){
        Vector3 AP = P - A; //position of object to point
        Vector3 AB = B - A; 
        float magnitudeAB = AB.sqrMagnitude; 
        float ABAPproduct = Vector3.Dot(AP, AB); 
        float distance = ABAPproduct / magnitudeAB; //distance nomalization
        if (distance < 0) 
            return A;
        else if (distance > 1) 
            return B;
        else
            return A + AB * distance;//Closest point vector
    }

    public bool isFinish()
    {
        switch (HandSide.HS)
        {
            case whichSide.left:
                if(dottedData.data.points[dottedData.data.points.Length-1].x <= lineRenderer.GetPosition(lineRenderer.positionCount - 1).x) {
                    return true;
                }
                return false;
            
            case whichSide.right:
                if(dottedData.data.points[0].x >= lineRenderer.GetPosition(lineRenderer.positionCount - 1).x) {
                    return true;
                }
                return false;
        }
        return false;
    }
}
