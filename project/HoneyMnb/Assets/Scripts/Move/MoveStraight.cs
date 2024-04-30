using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pointsData;

public class MoveStraight : Move
{
    public GameObject hand;
    public override void InitSetting()
    {
        pointsArray = new PointsData();
        data.moveSpeed = 1.0f;
        data.maxDistance = 1.5f;
        data.scanningDistance = 1.5f;
        data.numPoint = pointsArray.straightNum;
        data.position = hand.transform.position;//잡았을 때의 손의 위치.
        data.isPreviousPosSet = false;
    }
    
    void Update(){
        data.position = hand.transform.position;
        Rotation();
    }
}
