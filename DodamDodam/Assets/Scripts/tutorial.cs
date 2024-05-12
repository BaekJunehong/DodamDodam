using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandUtils;
using handSide;
using TMPro;

public class tutorial : MonoBehaviour
{
    //1. 손으로 가위를 잡으세요
    //2. 가위를 분홍색 점에 가져가세요
    //3. 가위질을 해보세요(가위가 멈춤)
    //4. 2 반복
    //5. 손을 움직여 방향을 조절해보세요
    //6. 붉은 지점을 향해 가위질을 해보세요
    //7. 튜토리얼은 끝났습니다. 목표 지점을 향해 가위질을 해보세요!
    //8. 완료
    // Start is called before the first frame update

    [Header("Object Property")]
    public GameObject handObject;
    public GameObject scissorsObject;
    public GameObject cuttedPointObject;
    public GameObject guideLineObjcet;
    public GameObject destinationObject;
    public TextMeshProUGUI instructionText;

    [Header("Variable Property")]
    public float scanningDistance = 0.7f;
    public float moveSpeed = 10f;

    private int step = 0;
    private bool isGrabbedScissors = false;
    private hand Hand;
    private HandTracker _handtracker;
    private LineRenderer cuttedLineRenderer;
    private float time = 0;

    void Start()
    {
        cuttedLineRenderer = cuttedPointObject.GetComponent<LineRenderer>();
        _handtracker = gameObject.AddComponent<HandTracker>();

        Hand = FindObjectOfType<hand>();
        if(Hand != null){
            Hand.isHold += (isGrabbed)=> {
                if(isGrabbed && Vector3.Distance(handObject.transform.position, scissorsObject.transform.position) <= scanningDistance){
                    isGrabbedScissors = true;
                }
                else{
                    isGrabbedScissors = false;
                }
            };
        }
        cuttedPointObject.transform.position = HandSide.HS == whichSide.right ? new Vector3(7, 0, 0) : new Vector3(-7, 0, 0);
        destinationObject.transform.position = HandSide.HS == whichSide.right ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0);

        instructionText.text = "1. 손으로 가위를 잡으세요.";
    }

    

    // Update is called once per frame
    void Update()
    {
        gameFlow();
    }

    public void ObjectMove(){
        if(isGrabbedScissors){
            scissorsObject.transform.position = Vector3.MoveTowards(scissorsObject.transform.position, handObject.transform.position, moveSpeed * Time.deltaTime);
        }
    }
    private bool timeSleep(float timeLimit){
        time += Time.deltaTime;
            if(time >= timeLimit){
                time = 0;
                return true;
            }
            else{
                return false;
            }
    }

    private void gameFlow(){
        Vector3 direction = _handtracker.GetDirection();
        int power = _handtracker.Cutting();
        switch(step){
            case -1:
            //씬 넘기기
            break;
            case 0:
            //손으로 가위를 잡아야 하는 단계
            if(isGrabbedScissors){
                //성공을 출력
                step = 1;
                instructionText.text = "2. 가위를 분홍색 점에 가져가보세요.";
                print("2. 가위를 분홍색 점에 가져가보세요.");
                cuttedPointObject.SetActive(true);
            }
            break;
            case 1:
            //가위를 분홍색 점에 가져가는 단계
            ObjectMove();
            if(Vector3.Distance(scissorsObject.transform.position, cuttedPointObject.transform.position) <= 0.3f){
                //성공을 출력
                step = 2;
                print("가위질을 해보세요");
            }
            break;
            case 2:
            //가위질을 해보세요(가위가 멈춤)
            if(power >= 1 && isGrabbedScissors){
                cuttedLineRenderer.positionCount = 2;
                cuttedLineRenderer.SetPosition(0, cuttedPointObject.transform.position);
                cuttedPointObject.transform.position = cuttedPointObject.transform.position + Vector3.left * 3;
                cuttedLineRenderer.SetPosition(1, cuttedPointObject.transform.position);
                //성공을 출력
                step = 3;
                instructionText.text = "3. 가위를 분홍색 점에 가져가보세요.";
                print("가위를 분홍색 점에 가져가보세요!");
            }
            break;
            case 3:
            //가위를 분홍색 점에 가져가는 단계
            ObjectMove();
            if(Vector3.Distance(scissorsObject.transform.position, cuttedPointObject.transform.position) <= 0.3f){
                //성공을 출력
                step = 4;
                instructionText.text = "4. 손을 움직여 방향을 조절해보세요.";
                print("손을 움직여 방향을 조절해보세요!");
            }
            break;
            case 4:
            //손을 움직여 방향을 조절해보세요!
            guideLineObjcet.SetActive(true);
            if(timeSleep(1)){
                step = 5;
                instructionText.text = "5. 분홍색 점을 향해 가위질을 해보세요.";
                print("분홍색 점을 향해 가위질을 해보세요");
                destinationObject.SetActive(true);
            }
            break;
            case 5:
            //붉은 지점을 향해 가위질을 해보세요
            if(power >= 1 && Vector3.Distance(destinationObject.transform.position, cuttedPointObject.transform.position + direction * 3)<= 0.5f && isGrabbedScissors){
                cuttedLineRenderer.positionCount++;
                cuttedPointObject.transform.position = destinationObject.transform.position;
                cuttedLineRenderer.SetPosition(cuttedLineRenderer.positionCount - 1, cuttedPointObject.transform.position);
                //성공을 출력
                step = 6;
                instructionText.text = "6. 가위를 분홍색 점에 가져가보세요.";
                print("가위를 분홍색 점에 가져가보세요!");
                destinationObject.SetActive(false);
            }
            break;
            case 6:
            //가위를 분홍색 점에 가져가는 단계
            ObjectMove();
            if(Vector3.Distance(scissorsObject.transform.position, cuttedPointObject.transform.position) <= 0.7f && isGrabbedScissors){
                //성공을 출력
                step = 7;
                instructionText.text = "7. 강하게 가위질해보세요.";
                print("강하게 가위질해보세요!");
            }
            break;
            case 7:
            //강하게 가위질해보세요!
            if(power >= 2 && isGrabbedScissors){
                cuttedLineRenderer.positionCount++;
                cuttedPointObject.transform.position += direction * 6;
                cuttedLineRenderer.SetPosition(cuttedLineRenderer.positionCount - 1, cuttedPointObject.transform.position);
                //성공을 출력
                step = -1;
            }
            
            break;
        }
    }
}
