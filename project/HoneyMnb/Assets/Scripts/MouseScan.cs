using UnityEngine;
public class MouseScan : MonoBehaviour
{
    
    public float moveSpeed = 1.0f;
    public float maxDistance  = 0.7f;
    private Vector3 closestPoint;
    private Vector3 previousPos;
    private Vector3 currentPos;
    private Vector3 lastdirection;
    private Vector3 direction;
    private bool isPreviousPosSet = false;
    
    private float scanningDistance = 0.7f;
    private float distance;
    private Vector3 mousePosition;

    private static int NumPoint = DottedLine.GetNumPoint();
    private Vector3[] points_zigzag = new Vector3[NumPoint];
    private Vector3[] points_straight = new Vector3[2];
    private Vector3[] points_curve = new Vector3[8];
    void Start() {
        for(int i=0; i<NumPoint; i++){
                points_zigzag[i] = i%2==0 ? new Vector3(2*i-8,-1f,0) : new Vector3(2*i-8,1f,0);
            }

        points_straight[0] = new Vector3(-8, 0, 0);
        points_straight[1] = new Vector3(8, 0, 0);

        points_curve[0] = new Vector3(-8, -2, 0);
        points_curve[1] = new Vector3(-4.001f, -2, 0);
        points_curve[2] = new Vector3(-4, 2, 0);
        points_curve[3] = new Vector3(-0.001f, 2, 0);
        points_curve[4] = new Vector3(0, -2, 0);
        points_curve[5] = new Vector3(4, -2, 0);
        points_curve[6] = new Vector3(4.001f, 2, 0);
        points_curve[7] = new Vector3(8, 2, 0);
    }
    void Update()
{
    currentPos = transform.position; // 현재 위치를 항상 현재의 transform.position으로 설정

        if (!isPreviousPosSet) {
        // 최초 실행 시, previousPos와 isPreviousPosSet 초기화
            previousPos = currentPos;
            isPreviousPosSet = true;
        } else if (previousPos != currentPos) {
        // transform.position에 변화가 있을 때만 direction 계산
        direction = (currentPos - previousPos).normalized;

        float angleRad = Mathf.Atan2(direction.y, direction.x);
        float angleDeg = angleRad * Mathf.Rad2Deg;
        angleDeg += 270;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);

        previousPos = currentPos; // 위치 업데이트
        }

        // 여기서는 distance와 mousePosition을 계산하는 로직이 계속 실행됩니다.
        distance = float.MaxValue;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        switching();
    }

    private void zigzagMove(){
        for (int i = 0; i < NumPoint - 1; i++)
        {
            Vector3 start = points_zigzag[i];
            Vector3 end = points_zigzag[i+1];
            Vector3 closestpoints = FindClosestPoint.ClosestPointOnLineSegment(start, end, mousePosition);
            float currentDistance = Vector3.Distance(mousePosition, closestpoints);
            if(currentDistance < distance){
                distance = currentDistance;
                closestPoint = closestpoints;
            }
        }
        if (distance <= maxDistance && Vector3.Distance(mousePosition, transform.position) <= scanningDistance && mousePosition.x < transform.position.x){
            transform.position = Vector3.MoveTowards(transform.position, closestPoint, moveSpeed * Time.deltaTime);
        }
    }

    private void straightMove(){
        points_straight[0] = new Vector3(-8, 0, 0);
        points_straight[1] = new Vector3(8, 0, 0);
        Vector3 start = points_straight[0];
        Vector3 end = points_straight[1];
        Vector3 closestpoints = FindClosestPoint.ClosestPointOnLineSegment(start, end, mousePosition);
        float currentDistance = Vector3.Distance(mousePosition, closestpoints);
        if(currentDistance < distance){
            distance = currentDistance;
            closestPoint = closestpoints;
        }
        if (distance <= maxDistance && Vector3.Distance(mousePosition, transform.position) <= scanningDistance && mousePosition.x < transform.position.x){
            transform.position = Vector3.MoveTowards(transform.position, closestPoint, moveSpeed * Time.deltaTime);
        }
    }

    private void curveMove(){
        
        for (int i = 0; i < 7; i++){
            Vector3 start = points_curve[i];
            Vector3 end = points_curve[i+1];
            Vector3 closestpoints = FindClosestPoint.ClosestPointOnLineSegment(start, end, mousePosition);
            float currentDistance = Vector3.Distance(mousePosition, closestpoints);
            Vector3 potentialDirection = (closestpoints - transform.position).normalized;
        if (currentDistance < distance && Vector3.Dot(lastdirection, potentialDirection) >= 0) { // 같은 방향 혹은 정지 상태 확인
            distance = currentDistance;
            closestPoint = closestpoints;
        }
        }
        if (distance <= maxDistance && Vector3.Distance(mousePosition, transform.position) <= scanningDistance && mousePosition.x < transform.position.x){
            Vector3 moveToDirection = (closestPoint - transform.position).normalized;
            if (moveToDirection != Vector3.zero) { // 실제 이동 방향이 있는지 확인
            transform.position = Vector3.MoveTowards(transform.position, closestPoint, moveSpeed * Time.deltaTime);
            lastdirection = moveToDirection; // 마지막 이동 방향 업데이트
        }
        }
    }

    private void switching(){
        switch(gameObject.name){
            case"StraightCutter":
            straightMove();
            break;
            case"ZigzagCutter":
            zigzagMove();
            break;
            case"CurveCutter":
            curveMove();
            break;
        }
    }


}
