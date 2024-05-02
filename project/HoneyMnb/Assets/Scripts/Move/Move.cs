using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ScanData{
    public float moveSpeed;
    public float maxDistance;
    public float scanningDistance;
    public int numPoint;
    public Vector3 position;
    public float distance;
    public Vector3 closestPoint;
    public Vector3 previousPos;
    public Vector3 currentPos;
    public bool isPreviousPosSet;

}

public abstract class Move : MonoBehaviour
{
    private hand Hand;
    public ScanData data;
    public abstract void InitSetting();

    public virtual void ObjectMove(Vector3[] points){
        data.distance = float.MaxValue;
        for (int i = 0; i < data.numPoint-1; i++){
            Vector3 start = points[i];
            Vector3 end = points[i+1];
            Vector3 closestpoints = ClosestPointOnLineSegment(start, end, data.position);
            float currentDistance = Vector3.Distance(data.position, closestpoints);
            if(currentDistance < data.distance){
                data.distance = currentDistance;
                data.closestPoint = closestpoints;
            }
        }
        if (data.distance <= data.maxDistance && Vector3.Distance(data.position, transform.position) <= data.scanningDistance && data.position.x < transform.position.x){
            transform.position = Vector3.MoveTowards(transform.position, data.closestPoint, data.moveSpeed * Time.deltaTime);
        }
    }

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

    public void Rotation(){
        data.currentPos = transform.position;
        if (!data.isPreviousPosSet) {
            // 최초 실행 시, previousPos와 isPreviousPosSet 초기화
            data.previousPos = data.currentPos;
            data.isPreviousPosSet = true;
        } else if (data.previousPos != data.currentPos) {
        // transform.position에 변화가 있을 때만 direction 계산
        Vector3 direction = (data.currentPos - data.previousPos).normalized;

        float angleRad = Mathf.Atan2(direction.y, direction.x);
        float angleDeg = angleRad * Mathf.Rad2Deg;
        angleDeg += 270;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);

        data.previousPos = data.currentPos; // 위치 업데이트
        }
    }
    protected virtual void Start(){
        Hand = FindObjectOfType<hand>();
        if(Hand != null){
            Hand.isHold += (isGrabbed)=> {
                if(isGrabbed){
                    ObjectMove(GetPoints());
                }
            };
        }
        InitSetting();
    }
    protected abstract Vector3[] GetPoints();
}
