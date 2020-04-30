using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// MRTKのEyeTrackingTargetコンポーネントを利用したサンプル
    /// 他のフォーカスポインターから完全に独立した目ベースの入力を処理できる
    /// https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/EyeTracking/EyeTracking_TargetSelection.html#2-independent-eye-gaze-specific-eyetrackingtarget
    /// </summary>
    public class EyeTrackingTargetSample : MonoBehaviour
    {
        [SerializeField] private TextMesh debugLabel = default;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void OnLookAtStart()
        {
            debugLabel.text = "OnLookAtStart";
        }

        public void WhileLookingAtTarget()
        {
            debugLabel.text = "WhileLookingAtTarget";
        }

        public void OnLookAway()
        {
            debugLabel.text = "OnLookAway";
        }

        public void OnDwell()
        {
            debugLabel.text = "OnDwell";
        }

        public void OnSelected()
        {
            debugLabel.text = "OnSelected";
        }
    }
}
