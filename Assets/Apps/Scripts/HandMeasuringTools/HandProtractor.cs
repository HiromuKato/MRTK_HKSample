using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// ハンド分度器
    /// 参考動画：https://twitter.com/hi_rom_/status/1267173213341052928
    /// </summary>
    public class HandProtractor : MonoBehaviour
    {
        [SerializeField]
        private TextMesh leftDegreeText = default;

        [SerializeField]
        private TextMesh rightDegreeText = default;

        [SerializeField]
        private List<LineRenderer> lines = new List<LineRenderer>(4);

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
            foreach (var line in lines)
            {
                line.SetPosition(0, Vector3.zero);
                line.SetPosition(1, Vector3.zero);
            }
            leftDegreeText.text = "0 degree";
            rightDegreeText.text = "0 degree";
        }

        void Update()
        {
            // 線の太さ
            foreach (var line in lines)
            {
                line.startWidth = 0.001f;
                line.endWidth = 0.001f;
            }

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

            // 左手 親指付け根
            var leftThumbMetacarpal = handJointService.RequestJointTransform(TrackedHandJoint.ThumbMetacarpalJoint, Handedness.Left);
            if (leftThumbMetacarpal == null)
            {
                Debug.Log("leftThumbMetacarpal is null.");
                return;
            }

            // 左手の線描画
            lines[0].SetPosition(0, leftIndexTip.position);
            lines[0].SetPosition(1, leftThumbMetacarpal.position);
            lines[1].SetPosition(0, leftThumbTip.position);
            lines[1].SetPosition(1, leftThumbMetacarpal.position);

            // 親指付け根を原点とした、人差し指と親指の角度算出
            var p0 = leftThumbMetacarpal.position;
            var p1 = leftIndexTip.position;
            var p2 = leftThumbTip.position;
            var v1 = p1 - p0;
            var v2 = p2 - p0;
            var angleLeft = Vector3.Angle(v2, v1);
            leftDegreeText.text = angleLeft.ToString("0.0") + " degree";
            leftDegreeText.transform.position = (leftIndexTip.position + leftThumbTip.position) / 2;

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

            // 右手 親指付け根
            var rightThumbMetacarpal = handJointService.RequestJointTransform(TrackedHandJoint.ThumbMetacarpalJoint, Handedness.Right);
            if (rightThumbMetacarpal == null)
            {
                Debug.Log("rightThumbMetacarpal is null.");
                return;
            }

            // 右手の線描画
            lines[2].SetPosition(0, rightIndexTip.position);
            lines[2].SetPosition(1, rightThumbMetacarpal.position);
            lines[3].SetPosition(0, rightThumbTip.position);
            lines[3].SetPosition(1, rightThumbMetacarpal.position);

            // 親指付け根を原点とした、人差し指と親指の角度算出
            p0 = rightThumbMetacarpal.position;
            p1 = rightIndexTip.position;
            p2 = rightThumbTip.position;
            v1 = p1 - p0;
            v2 = p2 - p0;
            var angleRight = Vector3.Angle(v2, v1);
            rightDegreeText.text = angleRight.ToString("0.0") + " degree";
            rightDegreeText.transform.position = (rightIndexTip.position + rightThumbTip.position) / 2;
        }
    }
}
