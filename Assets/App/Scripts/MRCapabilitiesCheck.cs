using System.Text;
using Microsoft.MixedReality.Toolkit;
using TMPro;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// Capabilities の確認サンプル
    /// https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/DetectingPlatformCapabilities.html
    /// </summary>
    public class MRCapabilitiesCheck : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshPro tmp = default;

        void Start()
        {
            IMixedRealityCapabilityCheck capabilityCheck = CoreServices.InputSystem as IMixedRealityCapabilityCheck;
            if (capabilityCheck == null)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append($"Capabilities Check\n\n");
            bool support = false;

            support = capabilityCheck.CheckCapability(MixedRealityCapability.ArticulatedHand);
            sb.Append($"ArticulatedHand : {support}\n");

            support = capabilityCheck.CheckCapability(MixedRealityCapability.EyeTracking);
            sb.Append($"EyeTracking : {support}\n");

            support = capabilityCheck.CheckCapability(MixedRealityCapability.GGVHand);
            sb.Append($"GGVHand : {support}\n");

            support = capabilityCheck.CheckCapability(MixedRealityCapability.MotionController);
            sb.Append($"MotionController : {support}\n");

            support = capabilityCheck.CheckCapability(MixedRealityCapability.VoiceCommand);
            sb.Append($"VoiceCommand : {support}\n");

            support = capabilityCheck.CheckCapability(MixedRealityCapability.VoiceDictation);
            sb.Append($"VoiceDictation : {support}\n");

            support = capabilityCheck.CheckCapability(MixedRealityCapability.SpatialAwarenessMesh);
            sb.Append($"SpatialAwarenessMesh : {support}\n");

            support = capabilityCheck.CheckCapability(MixedRealityCapability.SpatialAwarenessPlane);
            sb.Append($"SpatialAwarenessPlane : {support}\n");

            support = capabilityCheck.CheckCapability(MixedRealityCapability.SpatialAwarenessPoint);
            sb.Append($"SpatialAwarenessPoint : {support}\n");

            tmp.text = sb.ToString();
        }
    }
}
