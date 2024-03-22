using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string[] hand_data;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        double x = 0;
        double y = 0;
        
        hand_data = GameObject.Find("Player").GetComponent<Tracker>().points;
        
        // THUMB_TIP, INDEX_FINGER_TIP, MIDDLE_FINGER_TIP 의 좌표 
        for ( int i = 4; i < 16; i += 4) 
        {
            x += float.Parse(hand_data[i * 3]) * 100;
            y += 50 - float.Parse(hand_data[i * 3 + 1]) * 100;
        }
        x /= 3.0;
        y /= 3.0;
        transform.localPosition = new Vector3((float)x, (float)y, 0);
    }
}
