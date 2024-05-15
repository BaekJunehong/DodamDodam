using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pointsData{
    public class PointsData{
        public int straightNum = 2;
        public int zigzagNum_hard = 8;
        public int zigzagNum_easy = 5;
        public int zigzagNum_normal = 7;
        public int curveNum = 21;
        public Vector3[] points_straight;
        public Vector3[] points_straight_hard;
        public Vector3[] points_zigzag_easy;
        public Vector3[] points_zigzag_normal;
        public Vector3[] points_zigzag_hard;
        public Vector3[] points_curve_easy;
        public Vector3[] points_curve_normal;
        public Vector3[] points_curve_hard;

        public PointsData(){
            points_straight = new Vector3[straightNum];
            points_straight_hard = new Vector3[straightNum];
            points_zigzag_hard = new Vector3[zigzagNum_hard];
            points_zigzag_easy = new Vector3[zigzagNum_easy];
            points_zigzag_normal = new Vector3[zigzagNum_normal];
            points_curve_easy = new Vector3[curveNum];
            points_curve_normal = new Vector3[curveNum];
            points_curve_hard = new Vector3[curveNum];

            RectTransform paperRT = GameObject.Find("paper").GetComponent<RectTransform>();
            
            Vector2 rightTop = new Vector2((paperRT.rect.width / 2f)*0.9f, (paperRT.rect.height / 2f)*0.9f);

            Vector3 rightTopPosition = new Vector3(rightTop.x, rightTop.y, 0f);

            Vector3 worldPosition = paperRT.TransformPoint(rightTopPosition);

            float posX = worldPosition.x;
            float posY = worldPosition.y;

            points_straight[0] = new Vector3(-posX, 0, 0);
            points_straight[1] = new Vector3(posX, 0, 0);

            points_straight_hard[0] = new Vector3(-posX, posY*0.8f, 0);
            points_straight_hard[1] = new Vector3(posX, -posY*0.8f, 0);

            

            for(int i = 0; i < zigzagNum_easy; i++){
                points_zigzag_easy[i] = i%2==0 ? new Vector3((posX/2f)*i-posX,-1f,0) : new Vector3((posX/2f)*i-posX,1f,0);
            }

            for(int i = 0; i < zigzagNum_normal; i++){
                points_zigzag_normal[i] = i%2==0 ? new Vector3(posX*(float)i/3f-posX,-1f,0) : new Vector3(posX*(float)i/3f-posX,1f,0);
            }

            for(int i = 0; i < zigzagNum_hard; i++){
                points_zigzag_hard[i] = i%2==0 ? new Vector3((2*posX)/(zigzagNum_hard-1)*i-posX,-1.5f,0) : new Vector3((2*posX)/(zigzagNum_hard-1)*i-posX,1.5f,0);
            }

            for(int i = 0; i < curveNum; i++){
            float x = (i / (float)(curveNum-1) * 2f - 1f) * posX;
            float y = -1f / 32f * x * x + (posY/4f);
            points_curve_easy[i] = new Vector3(x, y, 0);
            }

            for(int i = 0; i < curveNum; i++){
            float x = (i / (float)(curveNum-1) * 2f - 1f) * posX;
            float y = -2f / 32f * x * x + (posY/2f);
            points_curve_normal[i] = new Vector3(x, y, 0);
            }

            for(int i = 0; i < curveNum; i++){
            float x = (i / (float)(curveNum-1) * 2f - 1f) * posX;
            float y = -3f / 32f * x * x + (3*posY/4);
            points_curve_hard[i] = new Vector3(x, y, 0);
            }
        }
        
    }
}
