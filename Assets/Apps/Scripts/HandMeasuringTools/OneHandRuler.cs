using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// 片手のハンド定規
    /// </summary>
    public class OneHandRuler : MonoBehaviour
    {
        [SerializeField]
        private TextMesh leftDistanceText = default;

        [SerializeField]
        private TextMesh rightDistanceText = default;

        [SerializeField]
        private LineRenderer leftLine = default;

        [SerializeField]
        private LineRenderer rightLine = default;

        private IMixedRealityHandJointService handJointService = null;
        private IMixedRealityDataProviderAccess dataProviderAccess = null;

        void Start()
        {
            handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
            if (handJointService == null)
            {
                Debug.LogError("Can't get IMixedRealityHandJointService.");
                return;
            }

            dataProviderAccess = CoreServices.InputSystem as IMixedRealityDataProviderAccess;
            if (dataProviderAccess == null)
            {
                Debug.LogError("Can't get IMixedRealityDataProviderAccess.");
                return;
            }

            // ハンドレイを非表示にする
            //PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);

            Initialize();
        }

        public void Initialize()
        {
            leftLine.SetPosition(0, Vector3.zero);
            leftLine.SetPosition(1, Vector3.zero);
            leftDistanceText.text = "0 cm";

            rightLine.SetPosition(0, Vector3.zero);
            rightLine.SetPosition(1, Vector3.zero);
            rightDistanceText.text = "0 cm";
        }

        void Update()
        {
            // 左手 人差し指
            var leftIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left);
            if (leftIndexTip == null)
            {
                Debug.Log("leftIndexTip is null.");
                return;
            }

            // 左手 親指
            var leftThumbTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Left);
            if (leftThumbTip == null)
            {
                Debug.Log("leftThumbTip is null.");
                return;
            }

            // 線を描画
            leftLine.SetPosition(0, leftIndexTip.position);
            leftLine.SetPosition(1, leftThumbTip.position);
            leftLine.startWidth = 0.001f;
            leftLine.endWidth = 0.001f;

            // 距離を算出
            var leftDistance = Vector3.Distance(leftIndexTip.position, leftThumbTip.position);
            // cmに変換
            leftDistance = leftDistance * 100;
            leftDistanceText.text = leftDistance.ToString("0.0") + " cm";
            leftDistanceText.transform.position = (leftIndexTip.position + leftThumbTip.position) / 2;

            // 右手 人差し指
            var rightIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
            if (rightIndexTip == null)
            {
                Debug.Log("rightIndexTip is null.");
                return;
            }

            // 右手 親指
            var rightThumbTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Right);
            if (rightThumbTip == null)
            {
                Debug.Log("rightThumbTip is null.");
                return;
            }

            // 線を描画
            rightLine.SetPosition(0, rightIndexTip.position);
            rightLine.SetPosition(1, rightThumbTip.position);
            rightLine.startWidth = 0.001f;
            rightLine.endWidth = 0.001f;

            // 距離を算出
            var rightDistance = Vector3.Distance(rightIndexTip.position, rightThumbTip.position);
            // cmに変換
            rightDistance = rightDistance * 100;
            rightDistanceText.text = rightDistance.ToString("0.0") + " cm";
            rightDistanceText.transform.position = (rightIndexTip.position + rightThumbTip.position) / 2;

        }
    }
}
