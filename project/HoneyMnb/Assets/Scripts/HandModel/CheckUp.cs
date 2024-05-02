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
        Vector3 direction = _handtracker.GetDirection();
        print(direction);
         _handtracker.DrawHand();

    }
}
