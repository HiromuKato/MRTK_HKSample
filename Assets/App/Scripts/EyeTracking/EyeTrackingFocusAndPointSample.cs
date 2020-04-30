using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// アイトラッキングの汎用のフォーカスおよびポインターハンドラーを使用するサンプル
    /// 参考：https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/EyeTracking/EyeTracking_TargetSelection.html
    /// </summary>
    public class EyeTrackingFocusAndPointSample : MonoBehaviour, IMixedRealityFocusHandler, IMixedRealityPointerHandler
    {
        [SerializeField]
        private TextMesh debugLabel = default;

        [SerializeField]
        private Material greenMat = default;
        [SerializeField]
        private Material blueMat = default;
        [SerializeField] 
        private Material redMat = default;

        // 選択アクションはUnity Editorで設定できるようにしておく
        [SerializeField]
        private MixedRealityInputAction selectAction = MixedRealityInputAction.None;

        private MeshRenderer mr;

        // Start is called before the first frame update
        void Start()
        {
            mr = gameObject.GetComponent<MeshRenderer>();
        }

        #region IMixedRealityFocusHandler
        // フォーカスの検出
        public void OnFocusEnter(FocusEventData eventData)
        {
            debugLabel.text = "OnFocusEnter";
            mr.material = greenMat;
        }

        public void OnFocusExit(FocusEventData eventData)
        {
            debugLabel.text = "OnFocusExit";
            mr.material = blueMat;
        }
        #endregion

        #region IMixedRealityPointerHandler
        // 単純なポインター入力イベントの検出
        public void OnPointerDown(MixedRealityPointerEventData eventData)
        {
            debugLabel.text = "OnPointerDown";
            // イベントをトリガーするために必要なアクション
            if (eventData.MixedRealityInputAction == selectAction)
            {
                mr.material = redMat;
            }
        }

        public void OnPointerDragged(MixedRealityPointerEventData eventData)
        {
            debugLabel.text = "OnPointerDragged";
        }

        public void OnPointerUp(MixedRealityPointerEventData eventData)
        {
            debugLabel.text = "OnPointerUp";
            mr.material = greenMat;
        }

        public void OnPointerClicked(MixedRealityPointerEventData eventData)
        {
            debugLabel.text = "OnPointerClicked";
        }
        #endregion
    }
}
