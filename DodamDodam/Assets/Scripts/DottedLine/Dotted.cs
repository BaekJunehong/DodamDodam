using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pointsData;
using difficulty;
using sceneData;
using gameCtrl;
using handSide;

public struct DottedData{
    public float width;
    public float textureScale;
    public int numPoint;
    public sceneType currentScene;
    public difficultyLevel difficulty;
    public Vector3[] points;
}

public abstract class Dotted : MonoBehaviour
{
    public PointsData pointsArray;
    public DottedData data;
    protected LineRenderer lineRenderer;
    private ChangeGame eventHandler;

    public virtual void InitSetting(){
        lineRenderer = GetComponent<LineRenderer>();
        pointsArray = new PointsData();
        data.width = 0.1f;
        data.textureScale = 0.5f;
        data.points = null;
    }

    public void DrawDottedLine(){
        data.currentScene = SceneData.SC;
        data.difficulty = Difficulty.DF;
        Vector3[] points = null;
        switch(data.currentScene){
            
            case sceneType.straight :
            data.numPoint = pointsArray.straightNum;
            data.points = data.difficulty == difficultyLevel.hard ? pointsArray.points_straight_hard : pointsArray.points_straight;
            break;
            case sceneType.curve :
            data.numPoint = pointsArray.curveNum;
            data.points = data.difficulty == difficultyLevel.easy ? pointsArray.points_curve_easy : data.difficulty == difficultyLevel.normal? pointsArray.points_curve_normal : pointsArray.points_curve_hard;
            break;
            case sceneType.zigzag :
            data.numPoint = data.difficulty == difficultyLevel.easy? pointsArray.zigzagNum_easy : data.difficulty == difficultyLevel.normal? pointsArray.zigzagNum_normal : pointsArray.zigzagNum_hard;
            data.points = data.difficulty == difficultyLevel.easy? pointsArray.points_zigzag_easy : data.difficulty == difficultyLevel.normal? pointsArray.points_zigzag_normal : pointsArray.points_zigzag_hard;
            break;
        }
        DrawLine(data.points);
    }

    public virtual void DrawLine(Vector3[] array){
        lineRenderer.startWidth = data.width;
        lineRenderer.endWidth = data.width;
        lineRenderer.sharedMaterial.mainTextureScale = new Vector2(data.textureScale / data.width, 1.0f);
        lineRenderer.positionCount = data.numPoint;
    }

    protected void Start(){
        eventHandler = FindObjectOfType<ChangeGame>();
        if(eventHandler != null){
            eventHandler.sceneChange += ()=> {
                DrawDottedLine();
            };
        }
        InitSetting();
        DrawDottedLine();
    }

    public Vector3 getLastPoint() {
        switch(HandSide.HS)
        {
            case whichSide.left:
                return data.points[data.points.Length-1];
                break;
            case whichSide.right:
                return data.points[0];
                break;
        }
        return new Vector3(0,0,0);
    }
}
