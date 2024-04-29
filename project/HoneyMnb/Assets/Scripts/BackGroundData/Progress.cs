using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{
    public Text progress;
    public GameObject scissors;
    // Start is called before the first frame update
    private float movedDistance = 0;
    private Vector3 previousPostion = Vector3.zero;
    bool flag = false;

    public float prog = 0;
    
    // Update is called once per frame
    void Update()
    {
        if(gameObject.name == "curveProgress"){PrintMovedDistance();}
        else{printProgress();}
    }
    private void printProgress(){
        Vector3 position = scissors.transform.position;
        prog = ((position.x -8) / -16) * 100;
        progress.text = string.Format("{0:N0}%", prog);
    }
    private void PrintMovedDistance(){
        if(!flag){
            previousPostion = scissors.transform.position;
            flag = true;
        }
        Vector3 currentScissorsPosition = scissors.transform.position;
        if(movedDistance >= 100 || currentScissorsPosition.x <= -8){
            prog = 100;
        }
        else{
            movedDistance += Vector3.Distance(previousPostion, currentScissorsPosition);
            prog = (movedDistance/27.75f) * 100;
        }
        progress.text = string.Format("{0:N0}%", prog);
        previousPostion = currentScissorsPosition;
    }
}
