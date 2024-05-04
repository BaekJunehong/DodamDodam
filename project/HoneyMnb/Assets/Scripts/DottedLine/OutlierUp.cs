using System;
using UnityEngine;
using difficulty;
using pointsData;
using sceneData;

public class OutlierUp : Dotted
{
    public override void DrawLine(Vector3[] array){
        base.DrawLine(array);
        Vector3[] arr = new Vector3[array.Length];
        if(SceneData.SC == sceneType.zigzag){
            for(int i=0; i < array.Length; i++){
                arr[i] = array[i] + Vector3.up * (float)data.difficulty * (float)Math.Sqrt(2);
            }
        }
        else{
            for(int i=0; i < array.Length; i++){
                arr[i] = array[i] + Vector3.up * (float)data.difficulty;
            }
        }
        lineRenderer.SetPositions(arr);
    }
}
