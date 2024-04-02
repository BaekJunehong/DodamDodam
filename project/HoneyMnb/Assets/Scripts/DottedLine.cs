using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public float width = 0.1f;
    public float textureScale = 0.5f;
    public static int numPoint = 9;
    
    
    private Vector3[] points = new Vector3[numPoint];
    

    
    // Start is called before the first frame update
    void Awake()
    {
        switching();
    }
    // Update is called once per frame

    /// <summary>
    /// 외부 클래스에 numPoint를 넘겨주는 함수
    /// </summary>
    public static int GetNumPoint(){
        return numPoint;
    }
    /// <summary>
    /// 점선을 그리는 함수
    /// </summary>

    private void switching()
    {
        switch(gameObject.name){
            case "Straight":
            DrawStragithLine();
            break;
            case "Zigzag":
            DrawZigzagLine();
            break;
            case "Curve":
            DrawCurveLine();
            break;
        }
    }

    private void DrawCurveLine(){
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.sharedMaterial.mainTextureScale = new Vector2(textureScale / width, 1.0f);
        lineRenderer.positionCount = 8;
        points[0] = new Vector3(-8, -2, 0);
        points[1] = new Vector3(-4.001f, -2, 0);
        points[2] = new Vector3(-4, 2, 0);
        points[3] = new Vector3(-0.001f, 2, 0);
        points[4] = new Vector3(0, -2, 0);
        points[5] = new Vector3(4, -2, 0);
        points[6] = new Vector3(4.001f, 2, 0);
        points[7] = new Vector3(8, 2, 0);
        lineRenderer.SetPositions(points);
    }

    private void DrawZigzagLine(){
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.sharedMaterial.mainTextureScale = new Vector2(textureScale / width, 1.0f);
        lineRenderer.positionCount = numPoint;

        for(int i=0; i<numPoint; i++){
            points[i] = i%2==0 ? new Vector3(2*i-8,-1f,0) : new Vector3(2*i-8,1f,0);
        }
        lineRenderer.SetPositions(points);
    }

    private void DrawStragithLine(){
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.sharedMaterial.mainTextureScale = new Vector2(textureScale / width, 1.0f);
        lineRenderer.positionCount = 2;
        points[0] = new Vector3(-8,0,0);
        points[1] = new Vector3(8,0,0);
        lineRenderer.SetPositions(points);
    }
}
