using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// 視線入力のみフォーカスに反応するのサンプル（ハンドレイには反応しない）
    /// https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/EyeTracking/EyeTracking_TargetSelection.html#eye-gaze-specific-baseeyefocushandler
    /// </summary>
    public class EyeTrackingBaseEyeFocusHandlerSample : BaseEyeFocusHandler
    {
        [SerializeField] 
        private TextMesh debugLabel = default;

        // Start is called before the first frame update
        void Start()
        {

        }

        /// <summary>
        /// Triggered once the eye gaze ray starts intersecting with this target's collider.
        /// </summary>
        protected override void OnEyeFocusStart()
        {
            debugLabel.text = "OnEyeFocusStart";
        }

        /// <summary>
        /// Triggered while the eye gaze ray is intersecting with this target's collider.
        /// </summary>
        protected override void OnEyeFocusStay()
        {
            debugLabel.text = "OnEyeFocusStay";
        }

        /// <summary>
        /// Triggered once the eye gaze ray stops intersecting with this target's collider.
        /// </summary>
        protected override void OnEyeFocusStop()
        {
            debugLabel.text = "OnEyeFocusStop";
        }

        /// <summary>
        /// Triggered once the eye gaze ray has intersected with this target's collider for a specified amount of time.
        /// </summary>
        protected override void OnEyeFocusDwell()
        {
            debugLabel.text = "OnEyeFocusDwell";
        }

    }
}
