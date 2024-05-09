using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pointsData;

public class MoveScissors : Move
{
    public GameObject hand;
    public override void InitSetting()
    {
        pointsArray = new PointsData();
        data.moveSpeed = 10f;
        data.scanningDistance = 0.7f;
        data.position = hand.transform.position;//잡았을 때의 손의 위치.
        data.isPreviousPosSet = false;
    }


    void Update(){
        data.position = hand.transform.position;
        Rotation();
    }
}
