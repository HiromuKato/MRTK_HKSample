using System;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine.Windows.WebCam;

namespace MRTK_HKSample
{
    /// <summary>
    /// 写真撮影をするためのクラス
    /// 参考：https://docs.unity3d.com/ja/2018.4/Manual/windowsholographic-photocapture.html
    /// </summary>
    public class PhotoCaptureExample : MonoBehaviour
    {
        [SerializeField] private TextMesh debug;

        PhotoCapture photoCaptureObject = null;
        Texture2D targetTexture = null;
        private Resolution cameraResolution;

        private bool photoTaking = false;

        void Start()
        {
            cameraResolution = PhotoCapture.SupportedResolutions
                .OrderByDescending((res) => res.width * res.height).First();
            targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        }

        // 写真撮影する
        public void TakePhoto()
        {
            if (photoTaking)
            {
                Debug.Log("写真撮影中");
                return;
            }
            photoTaking = true;
            debug.text = "写真撮影開始";

            try
            {
                // PhotoCapture オブジェクトを作成します
                PhotoCapture.CreateAsync(false, delegate(PhotoCapture captureObject)
                {
                    photoCaptureObject = captureObject;
                    CameraParameters cameraParameters = new CameraParameters();
                    cameraParameters.hologramOpacity = 0.0f;
                    cameraParameters.cameraResolutionWidth = cameraResolution.width;
                    cameraParameters.cameraResolutionHeight = cameraResolution.height;
                    cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

                    // カメラをアクティベートします
                    photoCaptureObject.StartPhotoModeAsync(cameraParameters,
                        delegate(PhotoCapture.PhotoCaptureResult result)
                        {
                            // 写真を撮ります
                            photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
                        });
                });
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                debug.text = ex.Message;
            }
        }

        private void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
        {
            // ターゲットテクスチャに RAW 画像データをコピーします
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);

            // テクスチャが適用されるゲームオブジェクトを作成
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
            quadRenderer.material = new Material(Shader.Find("Mixed Reality Toolkit/Standard"));

            Camera camera = Camera.main;
            quad.transform.parent = this.transform;
            //quad.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);
            quad.transform.localPosition = camera.transform.position + camera.transform.forward * 3;
            quad.transform.localRotation = camera.transform.localRotation;

            quadRenderer.material.SetTexture("_MainTex", targetTexture);

            // カメラを非アクティブにします
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }

        private void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
        {
            // photo capture のリソースをシャットダウンします
            photoCaptureObject.Dispose();
            photoCaptureObject = null;

            photoTaking = false;
            debug.text = "写真撮影終了";
        }
    }
}