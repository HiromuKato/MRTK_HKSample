using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// Xboxコントローラーのすべての入力を個別に判別するサンプル
    /// （１つのボタンに１つのアクションを割り当てるようInputActionとControllerMappingのプロファイル設定を行う）
    /// </summary>
    public class XboxControllerSample : 
        MonoBehaviour, 
        IMixedRealityInputHandler, 
        IMixedRealityInputHandler<float>,
        IMixedRealityInputHandler<Vector2>
    {
        [SerializeField] private TextMesh textMesh;

        private void OnEnable()
        {
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler<float>>(this);
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler<Vector2>>(this);
        }

        private void OnDisable()
        {
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler>(this);
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler<float>>(this);
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler<Vector2>>(this);
        }

        public void OnInputChanged(InputEventData<float> eventData)
        {
            if (eventData.MixedRealityInputAction.Description == "Xbox Left Trigger")
            {
                textMesh.text = "Left Trigger\n" + eventData.InputData;
            }
            else if (eventData.MixedRealityInputAction.Description == "Xbox Right Trigger")
            {
                textMesh.text = "Right Trigger\n" + eventData.InputData;
            }
        }

        public void OnInputChanged(InputEventData<Vector2> eventData)
        {
            if (eventData.MixedRealityInputAction.Description == "Xbox Left Thumbstick")
            {
                textMesh.text = "Left Thumbstick\n" + eventData.InputData;
            }
            else if (eventData.MixedRealityInputAction.Description == "Xbox Right Thumbstick")
            {
                textMesh.text = "Right Thumbstick\n" + eventData.InputData;
            }
            else if (eventData.MixedRealityInputAction.Description == "Xbox D-Pad")
            {
                textMesh.text = "D-Pad\n" + eventData.InputData;
            }
        }

        public void OnInputDown(InputEventData eventData)
        {
            if (eventData.MixedRealityInputAction.Description == "Xbox A")
            {
                textMesh.text = "A";
            }
            else if (eventData.MixedRealityInputAction.Description == "Xbox B")
            {
                textMesh.text = "B";
            }
            else if (eventData.MixedRealityInputAction.Description == "Xbox X")
            {
                textMesh.text = "X";
            }
            else if (eventData.MixedRealityInputAction.Description == "Xbox Y")
            {
                textMesh.text = "Y";
            }
            else if (eventData.MixedRealityInputAction.Description == "Xbox Left Thumbstick Click")
            {
                textMesh.text = "Left Stick Click";
            }
            else if (eventData.MixedRealityInputAction.Description == "Xbox Right Thumbstick Click")
            {
                textMesh.text = "Right Stick Click";
            }
            else if (eventData.MixedRealityInputAction.Description == "Xbox View")
            {
                textMesh.text = "View";
            }
            else if (eventData.MixedRealityInputAction.Description == "Xbox Menu")
            {
                textMesh.text = "Menu";
            }
            else if (eventData.MixedRealityInputAction.Description == "Xbox Left Bumper")
            {
                textMesh.text = "Left Bumper";

            }
            else if (eventData.MixedRealityInputAction.Description == "Xbox Right Bumper")
            {
                textMesh.text = "Right Bumper";
            }
        }

        public void OnInputUp(InputEventData eventData)
        {
        }

    } // class XboxControllerSample
} // namespace MRTK_HKSample
