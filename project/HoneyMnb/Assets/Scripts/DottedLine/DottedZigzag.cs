using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pointsData;

public class DottedZigzag : Dotted
{
    public override void InitSetting()
    {
        pointsArray = new PointsData();
        data.width = 0.1f;
        data.textureScale = 0.5f;
        data.numPoint = pointsArray.zigzagNum;
    }
    public override void DrawLine(){
        base.DrawLine();
        lineRenderer.SetPositions(pointsArray.points_zigzag);
    }

    void Start() {
        InitSetting();
        DrawLine();
    }
}
