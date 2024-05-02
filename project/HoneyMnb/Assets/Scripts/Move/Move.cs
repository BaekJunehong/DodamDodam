using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ScanData{
    public float moveSpeed;
    public float scanningDistance;
    public Vector3 position;
    public Vector3 previousPos;
    public Vector3 currentPos;
    public bool isPreviousPosSet;

}

public abstract class Move : MonoBehaviour
{
    private hand Hand;
    public ScanData data;
    public abstract void InitSetting();
    
    public virtual void ObjectMove(){
        if (Vector3.Distance(data.position, transform.position) <= data.scanningDistance){
            transform.position = Vector3.MoveTowards(transform.position, data.position, data.moveSpeed * Time.deltaTime);
        }
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
                    ObjectMove();
                }
            };
        }
        InitSetting();
    }
}
