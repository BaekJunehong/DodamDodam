using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;
using UnityEngine.Android;
using System.Collections;
using System.Linq;

namespace Klak.TestTools {

public sealed class ImageSource : MonoBehaviour
{
    #region Public property

    public Texture Texture => OutputBuffer;
    public Vector2Int OutputResolution => _outputResolution;

    #endregion

    #region Editable attributes

    // Source type options
    public enum SourceType { Texture, Video, Webcam, Card, Gradient }
    [SerializeField] SourceType _sourceType = SourceType.Card;

    // Texture mode options
    [SerializeField] Texture2D _texture = null;
    [SerializeField] string _textureUrl = null;

    // Video mode options
    [SerializeField] VideoClip _video = null;
    [SerializeField] string _videoUrl = null;

    // Webcam options
    // [SerializeField] string _webcamName = "";
    [SerializeField] Vector2Int _webcamResolution = new Vector2Int(1920, 1080);
    [SerializeField] int _webcamFrameRate = 30;

    // Output options
    [SerializeField] RenderTexture _outputTexture = null;
    [SerializeField] Vector2Int _outputResolution = new Vector2Int(1920, 1080);

    #endregion

    #region Package asset reference

    [SerializeField, HideInInspector] Shader _shader = null;

    #endregion

    #region Private members

    UnityWebRequest _webTexture;
    WebCamTexture _webcam;
    Material _material;
    RenderTexture _buffer;

    RenderTexture OutputBuffer
      => _outputTexture != null ? _outputTexture : _buffer;

    // Blit a texture into the output buffer with aspect ratio compensation.
    void Blit(Texture source, bool vflip = false)
    {
        if (source == null) return;

        var aspect1 = (float)source.width / source.height;
        var aspect2 = (float)OutputBuffer.width / OutputBuffer.height;

        var scale = new Vector2(aspect2 / aspect1, aspect1 / aspect2);
        scale = Vector2.Min(Vector2.one, scale);
        if (vflip) scale.y *= -1;

        var offset = (Vector2.one - scale) / 2;

        Graphics.Blit(source, OutputBuffer, scale, offset);
    }

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        // Allocate a render texture if no output texture has been given.
        if (_outputTexture == null)
            _buffer = new RenderTexture
              (_outputResolution.x, _outputResolution.y, 0);

        // Create a material for the shader (only on Card and Gradient)
        if (_sourceType == SourceType.Card || _sourceType == SourceType.Gradient)
            _material = new Material(_shader);

        // Texture source type:
        // Blit a given texture, or download a texture from a given URL.
        if (_sourceType == SourceType.Texture)
        {
            if (_texture != null)
            {
                Blit(_texture);
            }
            else
            {
                _webTexture = UnityWebRequestTexture.GetTexture(_textureUrl);
                _webTexture.SendWebRequest();
            }
        }

        // Video source type:
        // Add a video player component and play a given video clip with it.
        if (_sourceType == SourceType.Video)
        {
            var player = gameObject.AddComponent<VideoPlayer>();
            player.source =
              _video != null ? VideoSource.VideoClip : VideoSource.Url;
            player.clip = _video;
            player.url = _videoUrl;
            player.isLooping = true;
            player.renderMode = VideoRenderMode.APIOnly;
            player.Play();
        }

        // Webcam source type:
        // Create a WebCamTexture and start capturing.
        if (_sourceType == SourceType.Webcam)
        {
            StartCoroutine(InitializeWebcam());
        }

        // Card source type:
        // Run the card shader to generate a test card image.
        if (_sourceType == SourceType.Card)
        {
            var dims = new Vector2(OutputBuffer.width, OutputBuffer.height);
            _material.SetVector("_Resolution", dims);
            Graphics.Blit(null, OutputBuffer, _material, 0);
        }
    }

    IEnumerator InitializeWebcam(){
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            // 사용자의 권한 응답을 기다림
            while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                yield return new WaitForSeconds(0.1f);  // 응답 대기 시간, 필요에 따라 조정 가능
            }
        }

        // 권한 부여 후 웹캠 초기화 진행
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            int frontCameraIndex = -1;

            // 전면 카메라 찾기
            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i].isFrontFacing)
                {
                    frontCameraIndex = i;
                    break;
                }
            }

            // 전면 카메라가 존재하면 초기화 및 실행
            if (frontCameraIndex >= 0)
            {
                
                _webcam = new WebCamTexture(devices[frontCameraIndex].name, _webcamResolution.x, _webcamResolution.y, _webcamFrameRate);
                // _webcam = new WebCamTexture(devices[frontCameraIndex].name);
                // _webcam.requestedFPS = 30;
                _webcam.Play();
                Debug.Log("웹캠이 초기화되고 실행되었습니다.");
            }
            else
            {
                Debug.Log("전면 카메라를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.Log("카메라 접근 권한이 거부되었습니다.");
            // 권한 거부에 대한 추가적인 처리 로직
        }
    }

    void OnDestroy()
    {
        if (_webcam != null)
        {
            _webcam.Stop();
            WebCamTexture.Destroy(_webcam);
            _webcam = null;
        }
        if (_buffer != null) Destroy(_buffer);
        if (_material != null) Destroy(_material);
    }

    void Update()
    {
        if (_sourceType == SourceType.Video)
            Blit(GetComponent<VideoPlayer>().texture);

        if (_sourceType == SourceType.Webcam && _webcam.didUpdateThisFrame)
            Blit(_webcam, _webcam.videoVerticallyMirrored);

        // Asynchronous image downloading
        if (_webTexture != null && _webTexture.isDone)
        {
            var texture = DownloadHandlerTexture.GetContent(_webTexture);
            _webTexture.Dispose();
            _webTexture = null;
            Blit(texture);
            Destroy(texture);
        }

        if (_sourceType == SourceType.Gradient)
            Graphics.Blit(null, OutputBuffer, _material, 1);
    }

    #endregion
}

} // namespace Klak.TestTools
