using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandUtils {
public class HandTracker : MonoBehaviour
{
    private hand Hand;
    private int startflag;
    private int flag = 0;
    private float timer = 0f;
    private float inputDelay = 0.2f;
    private Vector3 CurrentVector = new Vector3(0, 0, 0);
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

        CurrentVector = new Vector3 (-newX, newY, newZ);

        return CurrentVector;
    }

    public Vector3 GetVertex(int index)
        => GetValid > 0.02f ? MappingVertex(HandAnimator.instance.GetPoint(index)) : CurrentVector;
 
    public Vector3 GetCenter
        => GetValid > 0.02f ? MappingVertex(HandAnimator.instance.GetPoint(5)) : CurrentVector;

    public bool IsHold()
    {
        Vector3 refVector = HandAnimator.instance.GetPoint(17) - HandAnimator.instance.GetPoint(0);    //1817
        Vector3 cVector = HandAnimator.instance.GetPoint(20) - HandAnimator.instance.GetPoint(17);      //1918
        float dotProduct = Vector3.Dot(refVector.normalized, cVector.normalized);
        float angle = Mathf.Acos(dotProduct);
        int angleInDegrees = ((int)(angle * Mathf.Rad2Deg) == 90) ? 0 : (int)(angle * Mathf.Rad2Deg);
        return (angleInDegrees > 20) ? true : false;
    }

    // public bool isAvailableCutting(Vector3 start, Vector3 dir, Vector3 close, float width)
    //     => Vector3.Distance(start + dir, close) <= width ? true : false; 


    public int Cutting()
    {
        int power = 0;
        Vector3 rfVector = HandAnimator.instance.GetPoint(9) - HandAnimator.instance.GetPoint(0);   //76
        Vector3 coVector = HandAnimator.instance.GetPoint(12) - HandAnimator.instance.GetPoint(9);   //65
        float dotProduct_ = Vector3.Dot(rfVector.normalized, coVector.normalized);
        float angle_ = Mathf.Acos(dotProduct_);
        int angleInDegree = ((int)(angle_ * Mathf.Rad2Deg) == 90) ? 0 : (int)(angle_ * Mathf.Rad2Deg);
        int refangle = Mathf.Clamp(angleInDegree / 10, 0, 5);
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
        else if (timer > 0.0001f)
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

    public Vector3 GetDirection()
    {
        Vector3 A = HandAnimator.instance.GetPoint(5);
        Vector3 B = HandAnimator.instance.GetPoint(17);
        Vector3 AB = A - B;
        Vector3 dir = new Vector3(-AB.x, AB.y, 0);
        return dir.normalized;

    }
}
}
