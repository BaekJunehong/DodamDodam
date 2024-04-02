using UnityEngine;

public class FindClosestPoint
{
    public static Vector3 ClosestPointOnLineSegment(Vector3 A, Vector3 B, Vector3 P){
        Vector3 AP = P - A; //position of object to point
        Vector3 AB = B - A; 

        float magnitudeAB = AB.sqrMagnitude; 
        float ABAPproduct = Vector3.Dot(AP, AB); 
        float distance = ABAPproduct / magnitudeAB; //distance nomalization
        

        if (distance < 0) 
            return A;
        else if (distance > 1) 
            return B;
        else
            return A + AB * distance;//Closest point vector
    }
}
