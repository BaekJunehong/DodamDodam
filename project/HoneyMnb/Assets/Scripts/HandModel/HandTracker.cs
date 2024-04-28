using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandUtils {
public class HandTracker : MonoBehaviour
{
    int flag;
    float timer = 0f;
    float inputDelay = 0.2f;
    // int flag = 0;
    // int tempflag = 0;
    float maxDegree = 0;
    Vector3 CurrentVector = new Vector3(0, 0, 0);

    public float GetValid
        => HandAnimator.instance.GetScore;

    public float GetValidHand
        => HandAnimator.instance.GetHandedness;

    public void DrawHand()
        => HandAnimator.instance.OnRenderHand();

    public Vector3 MappingVertex(Vector3 refVec)
    {
        Vector3 originalMin = new Vector3(-0.85f, -0.5f, 0);
        Vector3 originalMax = new Vector3(0.85f, 0.5f, 0);

        Vector3 newMin = new Vector3(-9f, -5f, 0);
        Vector3 newMax = new Vector3(9f, 5f, 0);

        float xRatio = (refVec.x - originalMin.x) / (originalMax.x - originalMin.x);
        float yRatio = (refVec.y - originalMin.y) / (originalMax.y - originalMin.y);
        // float zRatio = (refVec.z - originalMin.z) / (originalMax.z - originalMin.z);

        float newX = Mathf.Clamp(newMin.x + xRatio * (newMax.x - newMin.x), -9f, 9f);
        float newY = Mathf.Clamp(newMin.y + yRatio * (newMax.y - newMin.y), -5f, 5f);
        // float newZ = newMin.z + zRatio * (newMax.z - newMin.z);
        float newZ = 0f;

        CurrentVector = new Vector3 (newX, newY, newZ);

        return CurrentVector;
    }

    public Vector3 GetVertex(int index)
        => GetValid > 0.02f ? MappingVertex(HandAnimator.instance.GetPoint(index)) : CurrentVector;
    //{
        // Matrix4x4 m = Matrix4x4.TRS(HandAnimator.instance.GetPoint(index),
        //                             Quaternion.identity, Vector3.one * 0.07f);
        // return m.MultiplyPoint3x4(HandAnimator.instance.GetPoint(index));
        // // return transform.TransformPoint(hand_data);
        // Vector3 handy_data = this.transform.TransformVector(hand_data);
        // // return hand_data;
        // return handy_data;
    //}
    public Vector3 GetCenter
        => GetValid > 0.02f ? MappingVertex((HandAnimator.instance.GetPoint(4) +
                                             HandAnimator.instance.GetPoint(8) +
                                             HandAnimator.instance.GetPoint(12)) / 3) : CurrentVector;

    public bool IsHold(GameObject Scissors)
    {
        Transform obj = Scissors.transform;
        if (Vector3.Distance(CurrentVector, obj.position) <= 1f)
        {
            Vector3 refVector = HandAnimator.instance.GetPoint(18) - HandAnimator.instance.GetPoint(17);
            Vector3 cVector = HandAnimator.instance.GetPoint(19) - HandAnimator.instance.GetPoint(18);
            float dotProduct = Vector3.Dot(refVector.normalized, cVector.normalized);
            float angle = Mathf.Acos(dotProduct);
            int angleInDegrees = ((int)(angle * Mathf.Rad2Deg) == 90) ? 0 : (int)(angle * Mathf.Rad2Deg);
            return (angleInDegrees > 30) ? true : false;
        }
        else return false;
    }

    public int Cutting()
    {
        // bool init = true;
        // float timer = 0f;
        // float inputDelay = 0.2f;
        Vector3 rfVector = HandAnimator.instance.GetPoint(7) - HandAnimator.instance.GetPoint(6);
        Vector3 coVector = HandAnimator.instance.GetPoint(6) - HandAnimator.instance.GetPoint(5);
        float dotProduct_ = Vector3.Dot(rfVector.normalized, coVector.normalized);
        float angle_ = Mathf.Acos(dotProduct_);
        int angleInDegree = ((int)(angle_ * Mathf.Rad2Deg) == 90) ? 0 : (int)(angle_ * Mathf.Rad2Deg);
        // int flag = 0;
        // int tempflag;

        timer += Time.deltaTime;

        // if (tempflag == 0 && (angleInDegree < 10 || angleInDegree > 55))
        // {
        //     tempflag = 0;
        // }
        // else if (tempflag < 1 && angleInDegree > 10)
        // {
        //     tempflag = 1;
        // }
        // else if (tempflag < 2 && angleInDegree > 20)
        // {
        //     tempflag = 2;
        // }
        // else if (tempflag < 3 && angleInDegree > 30)
        // {
        //     tempflag = 3;
        // }
        // else if (tempflag < 4 && angleInDegree > 40)
        // {
        //     tempflag = 4;
        // }
        // else
        // {
        //     tempflag = 5;
        // }

        maxDegree = (maxDegree < angleInDegree) ? angleInDegree : maxDegree;

        if (timer >= inputDelay)
        {
            // flag = tempflag;
             timer = 0f;
            // tempflag = 0;
            // return flag;
            flag = (int)Mathf.Clamp((int)(maxDegree / 10) % 10, 0, 5);
            maxDegree = 0;
            return flag;
        }
        else return -1;
        // return angleInDegree;
    }

    public Vector3 GetDirection()
        => Vector3.ProjectOnPlane(HandAnimator.instance.GetPoint(5)
                                - HandAnimator.instance.GetPoint(17), Vector3.forward).normalized;

}
}



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class HandTracker : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         Vector3 hand_data = HandAnimator.instance.GetPoint(4);

//         transform.position = hand_data;
//         Debug.Log(hand_data);
//     }
// }
