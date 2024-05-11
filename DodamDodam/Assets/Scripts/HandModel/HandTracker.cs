using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandUtils {
public class HandTracker : MonoBehaviour
{
    private hand Hand;
    private HandAnimator HA;
    private int CenterIndex = 14;
    private int flag = 0;
    private bool cancut = false;
    private float timer = 0f;
    private float inputDelay = 0.1f;
    private float threshold = 0.0001f;

    private Vector3 CurrentVector = new Vector3(0, 0, 0);
    private Vector3 CurrentDirection = new Vector3(0, 0, 0);


    void Awake(){
        Hand = FindObjectOfType<hand>();
        HA = GameObject.Find("WebSource").GetComponent<HandAnimator>();
    }
    public float GetValid
        => HA.GetScore;

    public float GetValidHand
        => HA.GetHandedness;

    public void DrawHand()
        => HA.OnRenderHand();

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
        CurrentVector = MappingVertex(HA.GetPoint(index));
        return CurrentVector;
    }

    public Vector3 GetCenter()
    {
        if (!isHandexist()) return CurrentVector;
        CurrentVector = MappingVertex(HA.GetPoint(CenterIndex));
        return CurrentVector;
    }

    public bool isHandexist()
    {
        float v = GetValid;
        float h = GetValidHand;
        if (v < 0.5f || h < 0.5f)
        {
            if (v > 0.9f) return true;
            else return false;
        } return true;
    }
    public int anglecalc(int n)
    {
        Vector3 refVector = HA.GetPoint(n) - HA.GetPoint(0);
        Vector3 coVector = HA.GetPoint(n+3) - HA.GetPoint(n);
        float dotProduct = Vector3.Dot(refVector.normalized, coVector.normalized);
        float angle = Mathf.Acos(dotProduct);
        int angleInDegrees = Mathf.Abs(angle * Mathf.Rad2Deg - 90) < threshold ? 0 : (int)(angle * Mathf.Rad2Deg);
        return angleInDegrees;
    }

    public bool IsHold()
    {
        if (!isHandexist()) return false;

        for(int i = 13; i <= 17; i += 4)
        {
            if (anglecalc(i) > 30) return true;
        }
        return false;
    }


    public int Cutting()
    {
        float a = Vector3.Distance(HA.GetPoint(4), HA.GetPoint(12));
        float b = Vector3.Distance(HA.GetPoint(4), HA.GetPoint(8));
        float dist = a > b ? a : b;

        if (timer >= inputDelay)
        {
            int power = flag;
            flag = 0;
            timer = 0;
            cancut = false;
            return power;
        }
        else if (flag > 0)
        {
            if (b < 0.05f) flag = 2;
            timer += Time.deltaTime;
            return 0;
        }
        else if (a < 0.06f)
        {
            if (cancut)
            {
                flag =  b < 0.05f ? 2 : 1;
                timer += Time.deltaTime;
            }
            return 0;
        }
        else
        {
            if (dist > 0.08f) cancut = true;
            return 0;
        }
    }

    public Vector3 GetDirection()
    {
        if (!isHandexist()) return CurrentDirection;


        Vector3 A = HA.GetPoint(14);
        Vector3 B = HA.GetPoint(17);
        Vector3 AB = A - B;
        Vector3 dir = new Vector3(-AB.x, AB.y, 0);
        CurrentDirection = dir.normalized;
        return CurrentDirection;
    }

    void OnDestroy()
      => Destroy(HA);
}
}