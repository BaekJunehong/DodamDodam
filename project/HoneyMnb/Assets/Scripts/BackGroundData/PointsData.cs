using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pointsData{
    public class PointsData{
        public int straightNum = 2;
        public int zigzagNum = 9;
        public int curveNum = 11;
        public Vector3[] points_straight;
        public Vector3[] points_zigzag;
        public Vector3[] points_curve;

        public PointsData(){
            points_straight = new Vector3[straightNum];
            points_zigzag = new Vector3[zigzagNum];
            points_curve = new Vector3[curveNum];

            points_straight[0] = new Vector3(-8, 0, 0);
            points_straight[1] = new Vector3(8, 0, 0);

            for(int i = 0; i < zigzagNum; i++){
                points_zigzag[i] = i%2==0 ? new Vector3(2*i-8,-1f,0) : new Vector3(2*i-8,1f,0);
            }

            for(int i = 0; i < curveNum; i++){
            float x = (i / (float)(curveNum-1) * 2f - 1f) * 8f;
            float y = -1f / 32f * x * x + 2f;
            points_curve[i] = new Vector3(x, y, 0);
            }
        }
        
    }
}
