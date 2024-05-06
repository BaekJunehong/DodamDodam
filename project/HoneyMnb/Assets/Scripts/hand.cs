using UnityEngine;
using HandUtils;
using System;
using handSide;

public class hand : MonoBehaviour
{
    public event Action<bool> isHold;
    HandTracker _handtracker;
    public GameObject scissors;
    private Vector3 handPosition;
    public Sprite openhand;
    public Sprite holdhand;
    private float moveSpeed = 10f;
    void Start()
    {
        _handtracker = gameObject.AddComponent<HandTracker>();
    }
    void Update()
    {
        handPosition = _handtracker.GetCenter();
        bool isGrabbed = _handtracker.IsHold();
        isHold?.Invoke(isGrabbed);
        if(isGrabbed){
            ChangeStateToHold();
        }
        else{
            ChangeStateToOpen();
        }
        transform.position = Vector3.MoveTowards(transform.position, handPosition, moveSpeed * Time.deltaTime);
        transform.rotation = HandSide.HS == whichSide.right ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
    }
    public void ChangeStateToHold()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == openhand)
        {
            spriteRenderer.sprite = holdhand;  // 스프라이트 변경
        }
    }
    public void ChangeStateToOpen()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == holdhand)
        {
            spriteRenderer.sprite = openhand;  // 스프라이트 변경
        }
    }
}
