using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttedLine : MonoBehaviour
{
    public float Width = 0.1f;

    private LineRenderer lineRenderer;
    private float distanceThreshold = 0.01f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 1; // 시작 위치를 라인에 추가
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.startWidth = Width;
        lineRenderer.endWidth = Width;
    }

    void Update()
    {
        DrawPath();
    }
    void DrawPath()
    {
        // 마지막 점과 현재 오브젝트 위치 사이의 거리가 threshold보다 크면 새로운 점을 추가
        if (Vector3.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 1), transform.position) > distanceThreshold)
        {
            lineRenderer.positionCount += 1; // 점의 수를 증가시킴
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position); // 새로운 위치를 추가
        }
    }
}
