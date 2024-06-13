using UnityEngine;
using UnityEngine.UI;

public class ResizeParentAndChildren : MonoBehaviour
{
    // 부모 캔버스의 RectTransform 컴포넌트
    private RectTransform parentRectTransform;

    void Start()
    {
        // 부모 캔버스의 RectTransform 가져오기
        parentRectTransform = GetComponent<RectTransform>();

        // 초기화 시 부모와 자식 UI 요소들의 크기를 조절
        Resize();
    }

    void Update()
    {
        // 부모 캔버스의 크기가 변경될 때마다 부모와 자식 UI 요소들의 크기를 조절
        Resize();
    }

    void Resize()
    {
        // 부모 캔버스의 크기를 가져오기
        float parentWidth = parentRectTransform.rect.width;
        float parentHeight = parentRectTransform.rect.height;

        parentRectTransform.sizeDelta = new Vector2(parentWidth, parentHeight);
        // 부모 캔버스의 크기에 따라 부모와 모든 자식 UI 요소들의 크기 조절
        ResizeRectTransform(parentRectTransform, parentWidth, parentHeight);
    }

    // 재귀적으로 부모와 모든 자식 UI 요소들의 크기를 조절하는 함수
    void ResizeRectTransform(RectTransform rectTransform, float parentWidth, float parentHeight)
    {
        // 현재 RectTransform의 크기를 설정
        //rectTransform.sizeDelta = new Vector2(parentWidth, parentHeight);

        // 자식 UI 요소들에 대해 비율에 맞게 크기를 조절
        foreach (RectTransform childRectTransform in rectTransform)
        {
            // 원래 자식 크기에 비례하여 부모와 자식의 크기를 조절
            Vector2 originalSize = childRectTransform.sizeDelta;
            float scaleX = parentWidth / originalSize.x;
            float scaleY = parentHeight / originalSize.y;

            // 비율에 맞게 크기를 조절
            childRectTransform.sizeDelta = new Vector2(originalSize.x * scaleX, originalSize.y * scaleY);

            // 자식 UI 요소들에 대해 재귀적으로 함수 호출
            ResizeRectTransform(childRectTransform, parentWidth, parentHeight);
        }
    }
}