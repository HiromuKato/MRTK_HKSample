using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace MRTK_HKSample
{
    /// <summary>
    /// アプリ内から録画を開始・停止するサンプル
    /// </summary>
    public class RecordSample : MonoBehaviour
    {
        /// <summary>
        /// デバイスポータルのURL(Wi-Fi接続時)
        /// </summary>
        [SerializeField]
        private string url = ""; // ← ★ここを入力する（または入力UIを作成する）

        /// <summary>
        /// デバイスポータルのユーザ名
        /// </summary>
        [SerializeField]
        private string usr = ""; // ← ★ここを入力する（または入力UIを作成する）

        /// <summary>
        /// デバイスポータルのパスワード
        /// </summary>
        [SerializeField]
        private string pass = ""; // ← ★ここを入力する（または入力UIを作成する）

        /// <summary>
        /// Wi-Fi接続時に必要なトークン
        /// </summary>
        private string csrfToken = "";

        /// <summary>
        /// ホログラムをキャプチャするかどうか
        /// </summary>
        [SerializeField]
        [Tooltip("ホログラムをキャプチャするかどうか")]
        private bool holograms = true;

        /// <summary>
        /// カメラ画像をキャプチャするかどうか
        /// </summary>
        [SerializeField]
        [Tooltip("カメラ画像をキャプチャするかどうか")]
        private bool pvCamera = true;

        /// <summary>
        /// マイクを利用するかどうか
        /// </summary>
        [SerializeField]
        [Tooltip("マイクを利用するかどうか")]
        private bool micAudio = true;

        /// <summary>
        /// アプリケーションのオーディオを録音するかどうか
        /// </summary>
        [SerializeField]
        [Tooltip("アプリケーションのオーディオを録音するかどうか")]
        private bool appAudio = true;

        /// <summary>
        /// RenderFromCamera を有効にするかどうか
        /// </summary>
        [SerializeField]
        [Tooltip("RenderFromCamera を有効にするかどうか")]
        private bool renderFromCamera = true;

        /// <summary>
        /// ボタン内のテキスト
        /// </summary>
        [SerializeField]
        private TextMeshPro btnText = default;

        /// <summary>
        /// デバッグ用テキスト表示
        /// </summary>
        [SerializeField]
        private TextMeshPro debugtext = default;

        /// <summary>
        /// 録画中かどうか
        /// </summary>
        private bool isRecording = false;

        /// <summary>
        /// 初期化処理
        /// </summary>
        private void Start()
        {
            isRecording = false;
            btnText.text = "Record";
            StartCoroutine(AuthCoroutine());
        }

        /// <summary>
        /// ボタンが押されたときの処理
        /// </summary>
        public void ButtonPushed()
        {
            if(isRecording)
            {
                StopRecord();
            }
            else
            {
                StartRecord();
            }
        }

        /// <summary>
        /// 録画を開始する
        /// </summary>
        public void StartRecord()
        {
            if (isRecording)
            {
                return;
            }

            btnText.text = "Stop";
            StartCoroutine(StartRecordCoroutine());
        }

        /// <summary>
        /// 録画を停止する
        /// </summary>
        public void StopRecord()
        {
            if (!isRecording)
            {
                return;
            }
            btnText.text = "Record";
            StartCoroutine(StopRecordCoroutine());
        }

        /// <summary>
        /// 認証に必要なCSRF-Tokenを取得するリクエスト処理を行う
        /// </summary>
        private IEnumerator AuthCoroutine()
        {
            UnityWebRequest request = new UnityWebRequest("https://" + url, "GET");
            request.certificateHandler = new CertHandler();
            request.SetRequestHeader("authorization", MakeAuthorizationString(usr, pass));
            request.SetRequestHeader("Cookie", "CSRF-Token=");
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                debugtext.text = request.error;
            }
            else
            {
                debugtext.text = "Authentication response received.";
                /*
                foreach (var h in request.GetResponseHeaders())
                {
                    Debug.Log($"{h.Key}: {h.Value}");
                }
                */

                csrfToken = request.GetResponseHeader("set-cookie");
            }
        }

        /// <summary>
        /// 録画開始のPOSTリクエスト処理を行う
        /// </summary>
        private IEnumerator StartRecordCoroutine()
        {
            string api = "/api/holographic/mrc/video/control/start";
            string parameter =
                "?holo=" + holograms.ToString().ToLower() +
                "&pv=" + pvCamera.ToString().ToLower() +
                "&mic=" + micAudio.ToString().ToLower() +
                "&loopback=" + appAudio.ToString().ToLower() +
                "&RenderFromCamera=" + renderFromCamera.ToString().ToLower();

            UnityWebRequest request = new UnityWebRequest("https://" + url + api + parameter, "POST");
            request.certificateHandler = new CertHandler();
            request.SetRequestHeader("authorization", MakeAuthorizationString(usr, pass));
            request.SetRequestHeader("x-csrf-token", csrfToken.Replace("CSRF-Token=", ""));
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                debugtext.text = request.error;
            }
            else
            {
                debugtext.text = "Recording start response received.";
                isRecording = true;
            }
        }

        /// <summary>
        /// 録画停止のPOSTリクエスト処理を行う
        /// </summary>
        private IEnumerator StopRecordCoroutine()
        {
            string api = "/api/holographic/mrc/video/control/stop";
            UnityWebRequest request = new UnityWebRequest("https://" + url + api, "POST");
            request.certificateHandler = new CertHandler();
            request.SetRequestHeader("authorization", MakeAuthorizationString(usr, pass));
            request.SetRequestHeader("x-csrf-token", csrfToken.Replace("CSRF-Token=", ""));
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                debugtext.text = request.error;
            }
            else
            {
                debugtext.text = "Recording stop response received.";
            }
            isRecording = false;
        }

        /// <summary>
        /// ユーザ情報からベーシック認証の認証文字列を生成する
        /// </summary>
        /// <param name="username">ユーザ名</param>
        /// <param name="password">パスワード</param>
        /// <returns>認証文字列</returns>
        private string MakeAuthorizationString(string username, string password)
        {
            string auth = username + ":" + password;
            auth = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(auth));
            auth = "Basic " + auth;
            Debug.Log(auth);
            return auth;
        }
    }

    /// <summary>
    /// 参考：https://forum.unity.com/threads/uniy-2018-2-https-webrequest-failes-on-latest-hololens-os-udpate.550429/
    /// 証明書の検証が無効になるので注意
    /// </summary>
    public class CertHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}
