using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// ハンド定規
    /// 参考動画：https://twitter.com/hi_rom_/status/1267100537578639363
    /// </summary>
    public class HandRuler : MonoBehaviour
    {
        [SerializeField]
        private TextMesh DistanceText = default;

        [SerializeField]
        private LineRenderer line = default;

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
            PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);
        }

        void Update()
        {
            // 左手
            var leftIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left);
            if (leftIndexTip == null)
            {
                Debug.Log("leftIndexTip is null.");
                return;
            }

            // 右手
            var rightIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
            if (rightIndexTip == null)
            {
                Debug.Log("rightIndexTip is null.");
                return;
            }

            // 線を描画
            line.SetPosition(0, leftIndexTip.position);
            line.SetPosition(1, rightIndexTip.position);
            line.startWidth = 0.001f;
            line.endWidth = 0.001f;

            // 距離を算出
            var distance = Vector3.Distance(leftIndexTip.position, rightIndexTip.position);
            // cmに変換
            distance = distance * 100;
            DistanceText.text = distance.ToString("0.0") + " cm";
            DistanceText.transform.position = (leftIndexTip.position + rightIndexTip.position) / 2;
        }
    }
}
