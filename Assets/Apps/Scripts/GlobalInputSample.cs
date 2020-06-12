using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// グローバルインプットのサンプル（オブジェクトにフォーカスしていない状態でも入力を受け取る）
    /// </summary>
    public class GlobalInputSample : MonoBehaviour, IMixedRealityInputHandler
    {
        [SerializeField]
        private GameObject prefab = default;

        [SerializeField]
        private MixedRealityInputAction selectAction = MixedRealityInputAction.None;

        private void OnEnable()
        {
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
        }

        private void OnDisable()
        {
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler>(this);
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        public void OnInputDown(InputEventData eventData)
        {
            // Select と Grip Press のアクションが発火し２回呼ばれるためフィルタ
            if (eventData.MixedRealityInputAction == selectAction)
            {
                Debug.Log(eventData.InputSource.SourceName);
                Debug.Log(eventData.InputSource.SourceId);
                Debug.Log(eventData.MixedRealityInputAction.Id);

                var camera = Camera.main.transform;
                var pos = camera.position + camera.forward * 2;
                var rot = Quaternion.identity;
                Instantiate(prefab, pos, rot);
            }
        }

        public void OnInputUp(InputEventData eventData)
        {
        }
    }
}
