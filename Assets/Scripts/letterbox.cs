using UnityEngine;
using UnityEngine.UI;

public class letterbox : MonoBehaviour
{
    public Image image; // 이미지를 가진 UI 객체

    void Start()
    {
        ResizeImage();
    }

    void Update()
    {
        ResizeImage();
    }

    void ResizeImage()
    {
        if (image == null)
        {
            Debug.LogError("이미지가 할당되지 않았습니다.");
            return;
        }

        RectTransform rectTransform = image.rectTransform;
        RectTransform canvasRectTransform = image.canvas.GetComponent<RectTransform>();

        // 이미지가 채워질 수 있는 영역의 가로 및 세로 크기
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;

        // 이미지가 차지할 가로 및 세로 크기 설정
        float imageWidth = canvasWidth;
        float imageHeight = canvasHeight;

        // 이미지의 비율을 유지하도록 가로 또는 세로 중 하나를 맞춥니다.

        // 이미지의 크기 조정
        rectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);
    }
}