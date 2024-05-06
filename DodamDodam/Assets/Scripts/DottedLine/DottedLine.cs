using UnityEngine;
using handSide;
public class DottedLine : Dotted
{
    public override void DrawLine(Vector3[] array){
        base.DrawLine(array);
        lineRenderer.SetPositions(array);

        //CuttedPoint 위치를 옮겨준다
        GameObject cuttedPoint = GameObject.Find("CuttedPoint");
        int lastIndex = HandSide.HS == whichSide.right ? data.numPoint - 1 : 0;
        cuttedPoint.transform.position = array[lastIndex];
        LineRenderer cuttedline = cuttedPoint.GetComponent<LineRenderer>();
        cuttedline.positionCount = 1;
        cuttedline.SetPosition(0, cuttedPoint.transform.position);
    }
}
