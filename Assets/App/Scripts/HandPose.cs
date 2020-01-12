using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// 掌の位置・回転を取得するサンプル(実機でしか動作しないため注意)
    /// 参考：https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/Input/HandTracking.html
    /// </summary>
    public class HandPose : MonoBehaviour
    {
        [SerializeField]
        private TextMesh posText;

        [SerializeField]
        private TextMesh rotText;

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
                Transform jointTransform = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
                posText.text = jointTransform.position.ToString();
                rotText.text = jointTransform.rotation.eulerAngles.ToString();
            }
        }
    }
}
