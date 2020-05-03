using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// 右掌の位置・回転を取得するサンプル(実機でしか動作しないため注意)
    /// 参考：https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/Input/HandTracking.html
    /// </summary>
    public class HandPose : MonoBehaviour
    {
        [SerializeField]
        private TextMesh posText = default;

        [SerializeField]
        private TextMesh rotText = default;

        void Start()
        {
        }

        void Update()
        {
            IMixedRealityHandJointService handJointService = null;
            if (CoreServices.InputSystem != null)
            {
                var dataProviderAccess = CoreServices.InputSystem as IMixedRealityDataProviderAccess;
                if (dataProviderAccess != null)
                {
                    handJointService = dataProviderAccess.GetDataProvider<IMixedRealityHandJointService>();
                }
            }

            if (handJointService != null)
            {
                Transform jointTransform = handJointService.RequestJointTransform(TrackedHandJoint.Palm, Handedness.Right);
                var p = jointTransform.position;
                posText.text = "(" + p.x.ToString("F3") + ", " + p.y.ToString("F3") + ", " + p.z.ToString("F3") + ")";
                var r = jointTransform.rotation.eulerAngles;
                rotText.text = "(" + r.x.ToString("000") + ", " + r.y.ToString("000") + ", " + r.z.ToString("000") + ")";
            }
        }
    }
}
