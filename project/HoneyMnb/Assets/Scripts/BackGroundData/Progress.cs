using UnityEngine;
using UnityEngine.UI;
using handSide;

public class Progress : MonoBehaviour
{
    public Text progress;
    public GameObject cuttedPoint;
    private Vector3 previousPostion = Vector3.zero;
    public float prog = 0;
    
    
    // Update is called once per frame
    void Update()
    {
    }
    private void printProgress(){
        Vector3 position = cuttedPoint.transform.position;
        prog = HandSide.HS == whichSide.right ? ((position.x -8) / -16) * 100 : ((position.x +8) / 16) * 100;
        progress.text = string.Format("{0:N0}%", prog);
    }
}
