using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandUtils;

public class guideLine : MonoBehaviour
{
    // Start is called before the first frame update
    public float Width = 0.1f;
    private HandTracker _handtracker;

    private LineRenderer lineRenderer;

    void Start()
    {
        _handtracker = gameObject.AddComponent<HandTracker>();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.startWidth = Width;
        lineRenderer.endWidth = Width;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = _handtracker.GetDirection();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + direction * 2);
    }
}
