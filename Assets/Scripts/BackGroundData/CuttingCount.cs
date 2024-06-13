using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CuttingCount : MonoBehaviour
{
    public static int cuttingCount = 0;
    private CuttedPoint cuttingHandler;
    void Start() {
        //errorHandler
        cuttingHandler = FindObjectOfType<CuttedPoint>();
        if(cuttingHandler != null){
            cuttingHandler.CuttingCount += ()=> {
                cuttingCount++;
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int C{
        get => cuttingCount;
        set => cuttingCount = value;
    }
}
