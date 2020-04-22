using System;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Input;
using UnityEngine.XR.WSA.Persistence;

namespace MRTK_HKSample
{
    /// <summary>
    /// 空間アンカー(WorldAnchor)を管理するクラス
    /// このクラスで生成するアンカーは Anchor_1, Anchor_2 ... という名称(ID)となる
    /// WorldAnchorは実機でしか動作しないため注意
    /// </summary>
    public class WorldAnchorManager : MonoBehaviour
    {
        /// <summary>
        /// 配置場所を示すためのガイドオブジェクト
        /// </summary>
        [SerializeField]
        private GameObject guideObj = default;

        /// <summary>
        /// アンカーオブジェクトの親オブジェクト（このオブジェクト配下にアンカーオブジェクトを格納する）
        /// </summary>
        [SerializeField]
        private Transform worldAnchorsParent= default;

        /// <summary>
        /// storeから読み込んだWorldAnchorをアタッチするために生成するオブジェクトのPrefab
        /// </summary>
        [SerializeField]
        private GameObject attachObject = default;

        /// <summary>
        /// デバッグ表示用のテキスト
        /// </summary>
        [SerializeField]
        private TextMesh debugText = default;

        /// <summary>
        /// WorldAnchorを永続化するためのストレージオブジェクト
        /// </summary>
        private WorldAnchorStore store;

        /// <summary>
        /// ユーザーのジェスチャーを認識するためのAPIを持つマネージャクラス
        /// </summary>
        private GestureRecognizer recognizer;

        /// <summary>
        /// アンカー名のPrefix
        /// </summary>
        private readonly string anchorPrefix = "Anchor_";

        /// <summary>
        /// アンカー番号(保存時にIDが重複するのを防ぐために利用)
        /// </summary>
        private int anchorNum = 0;

        /// <summary>
        /// 既存のアンカー読み込みが終了したかどうかを保持する
        /// </summary>
        private bool loadFinished = false;

        #region MonoBehaviour method
        private void Start()
        {
#if !UNITY_WSA || UNITY_EDITOR
            Debug.LogWarning("WorldAnchor does not work with the Unity Editor.");
            return;
#else
            Initialize();
#endif
        }

        private void OnDestroy()
        {
            Terminate();
        }

        private void Update()
        {
            // 視線がヒットしたオブジェクトの位置にガイドオブジェクトを表示する
            var gazeRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            var distance = 20.0f;
            if (Physics.Raycast(gazeRay, out var hitInfo, distance))
            {
                guideObj.transform.position = hitInfo.point;
                guideObj.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
            }
        }
        #endregion

        #region Public method
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            loadFinished = false;
            anchorNum = 0;
            debugText.text = "";

            recognizer = new GestureRecognizer();
            recognizer.Tapped += HandleTap;
            recognizer.HoldCompleted += HandleHold;
            recognizer.StartCapturingGestures();

            WorldAnchorStore.GetAsync(StoreLoaded);
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Terminate()
        {
            if (recognizer == null)
            {
                return;
            }
            recognizer.Tapped -= HandleTap;
            recognizer.HoldCompleted -= HandleHold;
            recognizer.StopCapturingGestures();
        }

        /// <summary>
        /// アンカーを読み込む
        /// </summary>
        /// <param name="anchorId">アンカーID</param>
        /// <param name="go">アンカーを付けるオブジェクト</param>
        public void LoadAnchor(string anchorId, GameObject go)
        {
            var anchor = store.Load(anchorId, go);
            if (anchor == null)
            {
                debugText.text = "Error:アンカーの読み込みに失敗しました";
                return;
            }

            debugText.text += "アンカーを読み込みました:";
            debugText.text += anchor.name + "\n";
        }

        /// <summary>
        /// アンカーを配置後ストアへ保存する
        /// </summary>
        /// <param name="go">アンカーの付いたオブジェクト</param>
        public void SetAnchor(GameObject go)
        {
            WorldAnchor anchor = go.AddComponent<WorldAnchor>();
            anchor.name = go.name;
            if (anchor.isLocated)
            {
                SaveAnchor(anchor);
            }
            else
            {
                // アンカーが配置されない間は状態変更の通知を受け取る
                anchor.OnTrackingChanged += OnTrackingChanged;
            }
        }

        /// <summary>
        /// アンカーを保存する
        /// </summary>
        /// <param name="anchor">アンカー</param>
        public void SaveAnchor(WorldAnchor anchor)
        {
            var result = store.Save(anchor.name, anchor);
            if (!result)
            {
                // IDが重複している場合は保存できない
                debugText.text = "Error:アンカーを保存できませんでした";
                return;
            }

            debugText.text = "アンカーを保存しました:";
            debugText.text += anchor.name + "\n";
        }

        /// <summary>
        /// アンカーを削除する
        /// </summary>
        /// <param name="anchor">アンカー</param>
        public void DeleteAnchor(WorldAnchor anchor)
        {
            store?.Delete(anchor.name);
            DestroyImmediate(anchor);
        }

        /// <summary>
        /// アンカーをすべて削除する
        /// </summary>
        public void ClearAllAnchors()
        {
            store?.Clear();
        }
        #endregion

        #region Private method
        /// <summary>
        /// WorldAnchorStoreインスタンスが取得できたら呼ばれるデリゲート
        /// </summary>
        /// <param name="store">WorldAnchorStoreインスタンス</param>
        private void StoreLoaded(WorldAnchorStore store)
        {
            this.store = store;

            // 既存のアンカーを読み込む
            var maxNum = 0;
            string[] ids = this.store.GetAllIds();
            foreach (var id in ids)
            {
                var go = Instantiate(attachObject);
                go.transform.parent = worldAnchorsParent;
                go.name = id;
                if (go.name.StartsWith(anchorPrefix))
                {
                    // アンカー番号の最大インデックスを取得しておく
                    var countStr = go.name.Split('_')[1];
                    try
                    {
                        int.TryParse(countStr, out var num);
                        if (maxNum < num)
                        {
                            maxNum = num;
                        }
                    }
                    catch (Exception ex)
                    {
                        debugText.text = ex.Message;
                    }
                }

                LoadAnchor(id, go);

                // アンカー名の表示
                go.transform.Find("IDText").GetComponent<TextMesh>().text = id;
            }

            anchorNum = maxNum;
            loadFinished = true;
        }

        /// <summary>
        /// アンカーが配置可能かそうでないかの状態が変化したときに呼ばれる処理
        /// </summary>
        /// <param name="anchor">アンカー</param>
        /// <param name="located">配置されたかどうか</param>
        private void OnTrackingChanged(WorldAnchor anchor, bool located)
        {
            if (located)
            {
                SaveAnchor(anchor);
                anchor.OnTrackingChanged -= OnTrackingChanged;
            }
        }

        /// <summary>
        /// エアタップ時の処理
        /// </summary>
        /// <param name="tapEvent">タップイベント情報</param>
        private void HandleTap(TappedEventArgs tapEvent)
        {
            if (store == null || !loadFinished)
            {
                return;
            }

            var gazeRay = new Ray(tapEvent.headPose.position, tapEvent.headPose.forward);
            Physics.Raycast(gazeRay, out var hitInfo, float.MaxValue);

            var sphere = Instantiate(attachObject, hitInfo.point, Quaternion.identity);
            sphere.transform.parent = worldAnchorsParent;
            anchorNum++;
            sphere.name = anchorPrefix + anchorNum.ToString();
            SetAnchor(sphere);

            sphere.transform.Find("IDText").GetComponent<TextMesh>().text = sphere.name;
        }

        /// <summary>
        /// ホールド終了時の処理
        /// </summary>
        /// <param name="holdEvent">ホールドイベント情報</param>
        private void HandleHold(HoldCompletedEventArgs holdEvent)
        {
            if (store == null)
            {
                return;
            }

            ClearAllAnchors();
            anchorNum = 0;
            foreach (Transform child in worldAnchorsParent)
            {
                Destroy(child.gameObject);
            }

            debugText.text = "アンカーをすべて削除しました";
        }
        #endregion

    } // class WorldAnchorManager 
} // namespace MRTK_HKSample
