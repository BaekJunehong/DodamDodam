using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandUtils;

public class testvertex : MonoBehaviour
{
    // Start is called before the first frame update
    HandTracker _handtracker;
    void Start()
    {
        _handtracker = new HandTracker();
    }

    // Update is called once per frame
    void Update()
    {
        _handtracker.DrawHand();
        Vector3 test_data = _handtracker.GetCenter();
        // Debug.Log(test_data);
        // Debug.Log(_handtracker.GetValidHand);
        // Vector3 test_data = _handtracker.GetVertex(12);
        Debug.Log(test_data);
        // bool test_hold = _handtracker.IsHold();
        // Debug.Log(test_hold);
        // int test_angle = _handtracker.Cutting();
        // if ( test_angle != -1){
        //     Debug.Log(test_angle);
        // }
        // Debug.Log(test_angle);
        // Vector3 testt = _handtracker.GetDirection();
        // Debug.Log(testt);
    }
    
}
