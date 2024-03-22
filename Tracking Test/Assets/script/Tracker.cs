using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    // Start is called before the first frame update
    public UDPReceive udpReceive;
    public GameObject[] handPoints;
    public string[] points;
 
    // Update is called once per frame
    void Update()
    {
        string data = udpReceive.data;
 
        // 가지고 온 데이터에서 대괄호 [ ]를 제거.
        data = data.Remove(0, 1);
        data = data.Remove(data.Length-1, 1);
 
        // 쉼표를 기준으로 데이터를 분활
        points = data.Split(',');
 
        // 데이터에서 각 점의 좌표 출력
        for ( int i = 0; i < 21; i++)
        {
            float x = float.Parse(points[i * 3]) * 100;
            float y = 50 - float.Parse(points[i * 3 + 1]) * 100;
            // float z = float.Parse(points[i * 3 + 2]) * 100;
 
            handPoints[i].transform.localPosition = new Vector3(x, y, 0);
 
        }
        
    }
}
