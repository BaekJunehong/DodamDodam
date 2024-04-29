using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pointsData;

public class DottedCurve : Dotted
{
    
    public override void InitSetting()
    {
        pointsArray = new PointsData();
        data.width = 0.1f;
        data.textureScale = 0.5f;
        data.numPoint = pointsArray.curveNum;
    }

    public override void DrawLine(){
        base.DrawLine();
        lineRenderer.SetPositions(pointsArray.points_curve);
    }

    void Start() {
        InitSetting();
        DrawLine();
    }
}
