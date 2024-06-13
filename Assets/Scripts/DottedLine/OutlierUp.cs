using System;
using UnityEngine;
using difficulty;
using sceneData;

public class OutlierUp : Dotted
{
    public override void DrawLine(Vector3[] array){
        base.DrawLine(array);
        Vector3[] arr = new Vector3[array.Length];
        if(SceneData.SC == sceneType.zigzag){
            if(data.difficulty == difficultyLevel.easy){
                for(int i=0; i < array.Length; i++){
                    arr[i] = array[i] + Vector3.up * 2f * (float)Math.Sqrt(2);
                }
            }
            else if(data.difficulty == difficultyLevel.normal){
                for(int i=0; i < array.Length; i++){
                    arr[i] = array[i] + Vector3.up * 1f * (float)Math.Sqrt(2);
                }
            }
            else{
                for(int i=0; i < array.Length; i++){
                    arr[i] = array[i] + Vector3.up * (float)data.difficulty * (float)Math.Sqrt(2);
                }
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
