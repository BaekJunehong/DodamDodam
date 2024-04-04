using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

        Vector3 hand_data = GameObject.Find("Animator").GetComponent<HandAnimator>().GetPoint(4);
        
        // THUMB_TIP, INDEX_FINGER_TIP, MIDDLE_FINGER_TIP 의 좌표 
        Debug.Log(hand_data);
        
    }
}
