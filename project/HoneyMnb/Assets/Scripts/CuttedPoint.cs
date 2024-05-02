using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandUtils;

public class CuttedPoint : MonoBehaviour
{
    
    public float Width = 0.4f;
    public float difficulty = 3f;
    public GameObject objectHand;
    public GameObject scissors;
    public GameObject objectOfLine;

    private HandTracker _handtracker;
    private LineRenderer lineRenderer;
    private LineRenderer dottedLine;
    private hand Hand;
    private float distance;
    private Vector3 closestPoint;

    void Start()
    {
        dottedLine = objectOfLine.GetComponent<LineRenderer>();
        _handtracker = gameObject.AddComponent<HandTracker>();
        
        Hand = FindObjectOfType<hand>();
        if(Hand != null){
            Hand.isHold += (isGrabbed)=> {
                if(isGrabbed){
                    DrawPath();
                }
            };
        }
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 1; // 시작 위치를 라인에 추가
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.startWidth = Width;
        lineRenderer.endWidth = Width;
    }
    
    void DrawPath()
    {
        if(Vector3.Distance(scissors.transform.position, transform.position) <= 0.5f && Vector3.Distance(scissors.transform.position, objectHand.transform.position) <= 0.5f){//점과 가위의 거리가 가까울 때 발생한다.
            Vector3 direction = _handtracker.GetDirection();
            int power = _handtracker.Cutting();
            Vector3 destination = transform.position + direction * power;//목표점 계산
            
            int pointsCount = dottedLine.positionCount;
            Vector3[] points = new Vector3[pointsCount];
            dottedLine.GetPositions(points);//현재 위치한 씬의 점선 불러오기

            findClosestPointAndDistance(points, destination);
            //점선과의 거리가 난이도에 해당하는 굵기보다 작을 때만 그리기
            if(distance <= difficulty && 1 <= power){
                lineRenderer.positionCount += 1; // 점의 수를 증가시킴
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, destination); // 새로운 위치를 추가
                transform.position = destination;
            }

        }
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
}
