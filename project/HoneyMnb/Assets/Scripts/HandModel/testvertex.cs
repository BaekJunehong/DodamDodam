using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandUtils;

public class testvertex : MonoBehaviour
{
    public Camera camera;
    Vector3 test;
    Vector3 test_data;
    // Start is called before the first frame update
    HandTracker _handtracker;
    void Start()
    {
        _handtracker = new HandTracker();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 handPosition = _handtracker.GetVertex(4);
        Vector3 unityPosition = camera.WorldToScreenPoint(handPosition);
        // transform.position = test_data;
        Debug.Log(unityPosition);
    }
    
}
