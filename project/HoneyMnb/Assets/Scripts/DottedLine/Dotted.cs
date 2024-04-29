using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pointsData;

public struct DottedData{
    public float width;
    public float textureScale;
    public int numPoint;
}

public abstract class Dotted : MonoBehaviour
{
    public PointsData pointsArray;
    public DottedData data;
    protected LineRenderer lineRenderer;

    public abstract void InitSetting();

    public virtual void DrawLine(){
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = data.width;
        lineRenderer.endWidth = data.width;
        lineRenderer.sharedMaterial.mainTextureScale = new Vector2(data.textureScale / data.width, 1.0f);
        lineRenderer.positionCount = data.numPoint;
    }
}
