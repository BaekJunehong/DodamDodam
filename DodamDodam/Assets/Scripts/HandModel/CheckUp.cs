using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandUtils;

public class CheckUp : MonoBehaviour
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
        Vector3 middle_tip = _handtracker.GetVertex(12);
        Vector3 wrist = _handtracker.GetVertex(0);
        // Vector3 RAW_middle_tip = _handtracker.GetRawVertex(12);
        // Vector3 RAW_wrist = _handtracker.GetRawVertex(0);
    }
}
