using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandUtils {
public class HandTracker : MonoBehaviour
{
    public Vector3 GetVertex(int index)
        => HandAnimator.instance.GetPoint(index);
    //{
        // Matrix4x4 m = Matrix4x4.TRS(HandAnimator.instance.GetPoint(index),
        //                             Quaternion.identity, Vector3.one * 0.07f);
        // return m.MultiplyPoint3x4(HandAnimator.instance.GetPoint(index));
        // // return transform.TransformPoint(hand_data);
        // Vector3 handy_data = this.transform.TransformVector(hand_data);
        // // return hand_data;
        // return handy_data;
    //}
    
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
