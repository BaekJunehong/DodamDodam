using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStraight : Move
{
    public GameObject hand;
    public override void InitSetting()
    {
        data.moveSpeed = 5f;
        data.scanningDistance = 1.5f;
        data.position = hand.transform.position;//잡았을 때의 손의 위치.
        data.isPreviousPosSet = false;
    }

    
    void Update(){
        data.position = hand.transform.position;
        Rotation();
    }
}
