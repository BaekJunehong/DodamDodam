using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ErrorCount : MonoBehaviour
{
    public static int errorCount = 0;
    public Text error;
    private CuttedPoint errorHandler;
    void Start() {
        //errorHandler
        errorHandler = FindObjectOfType<CuttedPoint>();
        if(errorHandler != null){
            errorHandler.Error += ()=> {
                errorCount++;
            };
        }
    }
    void Update(){
        printError();
    }
    private void printError(){
        error.text = string.Format("{0}", errorCount);
    }

    public float E{
        get => errorCount;
    }
}
