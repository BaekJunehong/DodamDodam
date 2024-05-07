using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandUtils {
public class HandTracker : MonoBehaviour
{
    private hand Hand;
    private int CenterIndex = 5;
    private int flag = 0;
    private bool cancut = false;
    private float timer = 0f;
    private float inputDelay = 0.2f;
    private float threshold = 0.0001f;

    private Vector3 CurrentVector = new Vector3(0, 0, 0);
    private Vector3 CurrentDirection = new Vector3(0, 0, 0);


    void Awake(){
        Hand = FindObjectOfType<hand>();
    }
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

        Vector3 newMin = new Vector3(-18f, -10f, 0);       // (-9f, -5f, 0)  >> 200%
        Vector3 newMax = new Vector3(18f, 10f, 0);         // (9f, 5f, 0)  >> 200%

        float xRatio = (refVec.x - originalMin.x) / (originalMax.x - originalMin.x);
        float yRatio = (refVec.y - originalMin.y) / (originalMax.y - originalMin.y);
        // float zRatio = (refVec.z - originalMin.z) / (originalMax.z - originalMin.z);

        float newX = Mathf.Clamp(newMin.x + xRatio * (newMax.x - newMin.x), -9f, 9f);
        float newY = Mathf.Clamp(newMin.y + yRatio * (newMax.y - newMin.y), -5f, 5f);
        // float newZ = newMin.z + zRatio * (newMax.z - newMin.z);
        float newZ = 0f;

        Vector3 result = new Vector3 (-newX, newY, newZ);

        return result;
    }

    public Vector3 GetVertex(int index)
    {
        if (!isHandexist()) return CurrentVector;
        CurrentVector = MappingVertex(HandAnimator.instance.GetPoint(index));
        return CurrentVector;
    }

    public Vector3 GetCenter()
    {
        if (!isHandexist()) return CurrentVector;
        CurrentVector = MappingVertex(HandAnimator.instance.GetPoint(CenterIndex));
        return CurrentVector;
    }

    public bool isHandexist()
    {
        // return true;        // 테스트를 위한 작동 중지
        // Vector3 refVector = MappingVertex(HandAnimator.instance.GetPoint(CenterIndex));
        // float distance = Vector3.Distance(refVector, CurrentVector);
        // if (distance < threshold) return true;
        // return GetValid > 0.02f || distance >= 2f ? true : false;

        float v = GetValid;
        float h = GetValidHand;
        if (v < 0.5f || h < 0.5f)
        {
            if (v > 0.9f) return true;
            else return false;
        } return true;
        // return (v < 0.5f || h < 0.5f) ? false : true;
    }

    // public bool TrembleControl()
    // {
    //     return true;        // 테스트를 위한 작동 중지
    //     Vector3 refVector = MappingVertex(HandAnimator.instance.GetPoint(CenterIndex));
    //     float distance = Vector3.Distance(refVector, CurrentVector);
    //     if (distance < threshold) return true;
    //     return GetValid > 0.02f && distance >= 0.15f ? true : false;
    // }

    public int anglecalc(int n)
    {
        Vector3 refVector = HandAnimator.instance.GetPoint(n) - HandAnimator.instance.GetPoint(0);
        Vector3 coVector = HandAnimator.instance.GetPoint(n+3) - HandAnimator.instance.GetPoint(n);
        float dotProduct = Vector3.Dot(refVector.normalized, coVector.normalized);
        float angle = Mathf.Acos(dotProduct);
        int angleInDegrees = Mathf.Abs(angle * Mathf.Rad2Deg - 90) < threshold ? 0 : (int)(angle * Mathf.Rad2Deg);
        return angleInDegrees;
    }

    public bool IsHold()
    {
        if (!isHandexist()) return false;

        for(int i = 5; i <= 17; i += 4)
        {
            if (anglecalc(i) > 20) return true;
        }
        return false;
    }



    // public bool isAvailableCutting(Vector3 start, Vector3 dir, Vector3 close, float width)
    //     => Vector3.Distance(start + dir, close) <= width ? true : false; 


    // public int Cutting()        // 정도 구현 버전
    // {
    //     float a = Vector3.Distance(HandAnimator.instance.GetPoint(4), HandAnimator.instance.GetPoint(8));
    //     float b = Vector3.Distance(HandAnimator.instance.GetPoint(4), HandAnimator.instance.GetPoint(12));
    //     float dist = a > b ? a : b;

    //     if (timer >= inputDelay)
    //     {
    //         int power = flag;
    //         flag = 0;
    //         timer = 0;
    //         cancut = false;
    //         // Debug.Log(power);
    //         return power;
    //     }
    //     else if (flag > 0)
    //     {
    //         if (dist < 0.035f) flag = 3;
    //         else if (dist < 0.05f) flag = flag < 2 ? 2 : flag;
    //         timer += Time.deltaTime;
    //         return 0;
    //     }
    //     else if (dist < 0.075f)
    //     {
    //         if (cancut)
    //         {
    //             flag = 1;
    //             timer += Time.deltaTime;
    //         }
    //         return 0;
    //     }
    //     else
    //     {
    //         if (dist > 0.12f) cancut = true;    // 정면
    //         if (dist > 0.1f) cancut = true;     // 옆면
    //         return 0;
    //     }
    // }

    public int Cutting()            // t/f 버전
    {
        float a = Vector3.Distance(HandAnimator.instance.GetPoint(4), HandAnimator.instance.GetPoint(8));
        float b = Vector3.Distance(HandAnimator.instance.GetPoint(4), HandAnimator.instance.GetPoint(12));
        float dist = a > b ? a : b;

        if (timer >= inputDelay)
        {
            flag = 0;
            timer = 0;
            cancut = false;
            return 2;
        }
        else if (flag == 1)
        {
            timer += Time.deltaTime;
            return 0;
        }
        else if (dist < 0.07f)
        {
            if (cancut)
            {
                flag = 1;
                timer += Time.deltaTime;
            }
            return 0;
        }
        else
        {
            if (dist > 0.12f) cancut = true;       // 정면
            // if (dist > 0.1f) cancut = true;     // 옆면 
            return 0;
        }
    }

    public Vector3 GetDirection()
    {
        if (!isHandexist()) return CurrentDirection;

        Vector3 A = HandAnimator.instance.GetPoint(5);      // 5
        Vector3 B = HandAnimator.instance.GetPoint(17);     // 17
        Vector3 AB = A - B;
        Vector3 dir = new Vector3(-AB.x, AB.y, 0);
        CurrentDirection = dir.normalized;
        return CurrentDirection;
    }
}
}