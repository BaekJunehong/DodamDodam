using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandUtils {
public class HandTracker : MonoBehaviour
{
    private hand Hand;
    private int CenterIndex = 5;
    private int startflag;
    private int flag = 0;
    private float timer = 0f;
    private float inputDelay = 0.2f;
    private float threshold = 0.0001f;
    private float existtimer = 2f;
    private bool WaitCount = false;
    // private float HoldCount = 0f;
    private float CenterCount = 0f;
    private float DirectionCount = 0f;
    
    private int sensitivity = 15;
    private int init_value = 25;

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
        Vector3 newVector = MappingVertex(HandAnimator.instance.GetPoint(index));
        CurrentVector = isHandexist() ?  newVector : CurrentVector;
        return CurrentVector;
    }

    public Vector3 GetCenter()
    {
        if (isHandexist()) CenterCount = 0; else CenterCount += Time.deltaTime;
        if (CenterCount > existtimer) return CurrentVector;
        
        Vector3 newVector = MappingVertex(HandAnimator.instance.GetPoint(CenterIndex));
        CurrentVector = TrembleControl() ?  newVector : CurrentVector;
        return CurrentVector;
    }

    public bool isHandexist()
    {
        return true;        // 테스트를 위한 작동 중지
        Vector3 refVector = MappingVertex(HandAnimator.instance.GetPoint(CenterIndex));
        float distance = Vector3.Distance(refVector, CurrentVector);
        if (distance < threshold) return true;
        return GetValid > 0.02f || distance >= 2f ? true : false;
    }

    public bool TrembleControl()
    {
        return true;        // 테스트를 위한 작동 중지
        Vector3 refVector = MappingVertex(HandAnimator.instance.GetPoint(CenterIndex));
        float distance = Vector3.Distance(refVector, CurrentVector);
        if (distance < threshold) return true;
        return GetValid > 0.02f && distance >= 0.15f ? true : false;
    }

    public int anglecalc(int n)
    {
        Vector3 refVector = HandAnimator.instance.GetPoint(n) - HandAnimator.instance.GetPoint(0);
        Vector3 coVector = HandAnimator.instance.GetPoint(n+3) - HandAnimator.instance.GetPoint(n);
        float dotProduct = Vector3.Dot(refVector.normalized, coVector.normalized);
        float angle = Mathf.Acos(dotProduct);
        int angleInDegrees = Mathf.Abs(angle * Mathf.Rad2Deg - 90) < threshold ? 0 : (int)(angle * Mathf.Rad2Deg);
        return angleInDegrees;
    }

    public bool IsHold()            // 집게 동작 Hold
    {
        for(int i = 13; i <= 17; i += 4)
        {
            if (anglecalc(i) > 20) return true;
        }

        if(!WaitCount) WaitCount = true;
        return false;
    }

    // public bool IsHold()            // 악수 동작 Hold
    // {
    //     for(int i = 5; i <= 17; i += 4)
    //     {
    //         if (anglecalc(i) > 25) return true;
    //     }

    //     if(!WaitCount) WaitCount = true;
    //     return false;
    // }



    // public bool isAvailableCutting(Vector3 start, Vector3 dir, Vector3 close, float width)
    //     => Vector3.Distance(start + dir, close) <= width ? true : false; 

    public int Cutting()        // 집게 동작 Cutting
    {
        int power = 0;
        int refangle = 0;
        // Vector3 rfVector = HandAnimator.instance.GetPoint(9) - HandAnimator.instance.GetPoint(0);   //76
        // Vector3 coVector = HandAnimator.instance.GetPoint(12) - HandAnimator.instance.GetPoint(9);   //65
        // float dotProduct_ = Vector3.Dot(rfVector.normalized, coVector.normalized);
        // float angle_ = Mathf.Acos(dotProduct_);
        // int angleInDegree = Mathf.Abs(angle_ * Mathf.Rad2Deg - 90) < threshold ? 0 : (int)(angle_ * Mathf.Rad2Deg);
        // int refangle = Mathf.Clamp((angleInDegree - init_value) / sensitivity, 0, 5);
        for(int i = 5; i <= 9; i += 4)
        {
            int currentflag = Mathf.Clamp((anglecalc(i) - init_value) / sensitivity, 0, 5);
            refangle = refangle < currentflag ? currentflag : refangle; 
        }
        //핸들러로 손을 풀 때 처리
        Hand = FindObjectOfType<hand>();
        if(Hand != null){
            Hand.isHold += (isGrabbed)=> {
                if(!isGrabbed){
                    timer = 0f;
                }
            };
        }
        if (timer >= inputDelay)
        {
            flag = flag < refangle ? refangle : flag;
            timer = 0f;
            power = flag - startflag;
            startflag = refangle;
            return power;
        }
        else if (timer > threshold)
        {
            flag = flag < refangle ? refangle : flag;
            timer += Time.deltaTime;
        }
        else if (flag < refangle)
        {
            flag = refangle;
            timer += Time.deltaTime;    
        }
        else
        {
            flag = refangle;
            startflag = flag;
        } return power;
    }

    // public int Cutting()        // 악수 동작 Cutting
    // {
    //     int power = 0;
    //     int refangle = 0;

    //     for(int i = 5; i <= 17; i += 4)
    //     {
    //         int currentflag = Mathf.Clamp((anglecalc(i) - init_value) / sensitivity, 0, 5);
    //         refangle = refangle < currentflag ? currentflag : refangle; 
    //     }
    //     //핸들러로 손을 풀 때 처리
    //     Hand = FindObjectOfType<hand>();
    //     if(Hand != null){
    //         Hand.isHold += (isGrabbed)=> {
    //             if(!isGrabbed){
    //                 timer = 0f;
    //             }
    //         };
    //     }
    //     if (timer >= inputDelay)
    //     {
    //         flag = flag < refangle ? refangle : flag;
    //         timer = 0f;
    //         power = flag - startflag;
    //         startflag = refangle;
    //         return power;
    //     }
    //     else if (timer > threshold)
    //     {
    //         flag = flag < refangle ? refangle : flag;
    //         timer += Time.deltaTime;
    //     }
    //     else if (flag < refangle)
    //     {
    //         flag = refangle;
    //         timer += Time.deltaTime;    
    //     }
    //     else
    //     {
    //         flag = refangle;
    //         startflag = flag;
    //     } return power;
    // }

    public Vector3 GetDirection()
    {
        if (isHandexist()) DirectionCount = 0; else DirectionCount += Time.deltaTime;
        if (DirectionCount > existtimer) return CurrentDirection;

        Vector3 A = HandAnimator.instance.GetPoint(5);
        Vector3 B = HandAnimator.instance.GetPoint(17);
        Vector3 AB = A - B;
        Vector3 dir = new Vector3(-AB.x, AB.y, 0);
        CurrentDirection = TrembleControl() ? dir.normalized : CurrentDirection;
        return CurrentDirection;
    }
}
}