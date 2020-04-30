using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// アイトラッキングのサンプルクラス
    /// </summary>
    public class EyeTrackingSample : MonoBehaviour
    {
        [SerializeField]
        private Transform target = default;

        [SerializeField] 
        private float defaultDistanceInMeters = 2;

        [SerializeField] 
        private TextMesh label = default;

        private IMixedRealityEyeGazeProvider gazeProvider;

        // Start is called before the first frame update
        void Start()
        {
            gazeProvider = CoreServices.InputSystem.EyeGazeProvider;
        }

        public void CheckEyeGazeState()
        {
            // True if user has selected to use eye tracking for gaze.
            Debug.Log($"IsEyeGazeValid : {gazeProvider.IsEyeGazeValid}");

            // Indicates whether the user's eye tracking calibration is valid or not.
            Debug.Log($"IsEyeCalibrationValid : {gazeProvider.IsEyeCalibrationValid}");
        }

        // Update is called once per frame
        void Update()
        {
            if (!gazeProvider.IsEyeGazeValid)
            {
                label.text = $"IsEyeGazeValid : {gazeProvider.IsEyeGazeValid}";
                return;
            }

            if (gazeProvider.IsEyeCalibrationValid == null)
            {
                label.text = $"IsEyeCalibrationValid : null";
                return;
            }

            if (!(bool) gazeProvider.IsEyeCalibrationValid)
            {
                label.text = $"IsEyeCalibrationValid : {(bool) gazeProvider.IsEyeCalibrationValid}";
                return;
            }

            // Origin of the gaze ray.
            // Please note that this will return the head gaze origin if 'IsEyeGazeValid' is false.
            Debug.Log($"GazeOrigin : {gazeProvider.GazeOrigin}");
            label.text = $"GazeOrigin : {gazeProvider.GazeOrigin}\n";

            // Direction of the gaze ray.
            // This will return the head gaze direction if 'IsEyeGazeValid' is false.
            Debug.Log($"GazeDirection : {gazeProvider.GazeDirection}");
            label.text += $"GazeDirection : {gazeProvider.GazeDirection}\n";

            // Information about the currently gazed at target. Again, if IsEyeGazeValid is false,
            // this will be based on the user's head gaze.
            var hitInfo = gazeProvider.HitInfo;
            var hitPosition = gazeProvider.HitPosition;
            label.text += $"HitPosition : ({hitPosition.x}, {hitPosition.y}, {hitPosition.z})";
            var hitNormal = gazeProvider.HitNormal;

            if (gazeProvider.HitInfo.raycastValid)
            {
                // Show the object at the hit position of the user's eye gaze ray with the target.
                target.position = gazeProvider.HitPosition;
            }
            else
            {
                // If no target is hit, show the object at a default distance along the gaze ray.
                target.transform.position =
                    gazeProvider.GazeOrigin +
                    gazeProvider.GazeDirection.normalized * defaultDistanceInMeters;
            }

            label.text += $"raycastValid : {gazeProvider.HitInfo.raycastValid}";
        }

        /// <summary>
        /// ハンドレイをOnにする
        /// </summary>
        public void HandRayOn()
        {
            PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOn);

            // Restore default behavior for rays (on if not near something grabbable)
            //PointerUtils.SetHandRayPointerBehavior(PointerBehavior.Default);
        }

        /// <summary>
        /// ハンドレイをOffにする
        /// </summary>
        public void HandRayOff()
        {
            // Turn off all hand rays
            PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);

            // Turn off hand rays for the right hand only
            //PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff, Handedness.Right);
        }

        /// <summary>
        /// ゲイズポインターをOnにする
        /// </summary>
        public void GazePointerOn()
        {
            PointerUtils.SetGazePointerBehavior(PointerBehavior.AlwaysOn);
        }

        /// <summary>
        /// ゲイズポインターをOffにする
        /// </summary>
        public void GazePointerOff()
        {
            PointerUtils.SetGazePointerBehavior(PointerBehavior.AlwaysOff);
        }

    }
}
