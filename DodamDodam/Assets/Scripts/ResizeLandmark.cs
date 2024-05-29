using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeLandmark : MonoBehaviour
{
    Camera cam;
    public RectTransform border;
    public RectTransform center;
    // Start is called before the first frame update
    void Start()
    {
        float borderWidth = border.rect.width;
        float borderHeight = border.rect.height;
        float centerWidth = center.rect.width;
        float centerHeight = center.rect.height;

        float widthRatio = ((borderWidth - centerWidth) / (float)borderWidth) / 2f;
        float heightRatio = ((borderHeight - centerHeight) / (float)borderHeight) / 2f;

        cam = GetComponent<Camera>();
        cam.rect = new Rect(widthRatio, heightRatio, 1 - 2*widthRatio, 1 - 2*heightRatio);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
