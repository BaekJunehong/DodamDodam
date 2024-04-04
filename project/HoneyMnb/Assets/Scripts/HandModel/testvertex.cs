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
        Vector3 test_data = _handtracker.GetVertex(4);
        // transform.position = test_data;
        Debug.Log(test_data);
    }
}
