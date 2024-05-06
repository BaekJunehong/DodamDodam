using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sceneData;
using pointsData;
using difficulty;

public struct ScanData{
    public float moveSpeed;
    public float scanningDistance;
    public Vector3 position;
    public bool isPreviousPosSet;
    public sceneType currentScene;
    public difficultyLevel difficulty;
}

public abstract class Move : MonoBehaviour
{
    public PointsData pointsArray;
    private hand Hand;
    private Vector3 start;
    private Vector3 end;
    public ScanData data;
    public abstract void InitSetting();
    
    public virtual void ObjectMove(){
        if (Vector3.Distance(data.position, transform.position) <= data.scanningDistance){
            transform.position = Vector3.MoveTowards(transform.position, data.position, data.moveSpeed * Time.deltaTime);
        }
    }

    public void Rotation(){
        data.currentScene = SceneData.SC;
        data.difficulty = Difficulty.DF;
        Vector3[] points = null;
        switch(SceneData.SC){
            
            case sceneType.straight :
            points = data.difficulty == difficultyLevel.hard ? pointsArray.points_straight_hard : pointsArray.points_straight;
            break;
            case sceneType.curve :
            points = data.difficulty == difficultyLevel.easy ? pointsArray.points_curve_easy : data.difficulty == difficultyLevel.normal? pointsArray.points_curve_normal : pointsArray.points_curve_hard;
            break;
            case sceneType.zigzag :
            points = data.difficulty == difficultyLevel.easy? pointsArray.points_zigzag_easy : data.difficulty == difficultyLevel.normal? pointsArray.points_zigzag_normal : pointsArray.points_zigzag_hard;
            break;
        }
        if (points == null) {
            print("null!");
            return;
            }

        Vector3 direction = FindDirectionOnLine(points, transform.position);

        float angleRad = Mathf.Atan2(direction.y, direction.x);
        float angleDeg = angleRad * Mathf.Rad2Deg;
        angleDeg += 270;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);
        
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

    public Vector3 FindDirectionOnLine(Vector3[] arr, Vector3 P){
        for(int i=0; i<arr.Length-1; i++){
            if(arr[i].x <= P.x && P.x <= arr[i+1].x){
                start = arr[i];
                end = arr[i+1];
            }
        }
        return start - end;
    }
}
