using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// ハンドモデリングのサンプル
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class HandModelingSample : MonoBehaviour, IMixedRealityInputHandler
    {
        [SerializeField]
        private GameObject prefab = default;

        [SerializeField]
        private MixedRealityInputAction selectAction = MixedRealityInputAction.None;

        private IMixedRealityHandJointService handJointService = null;
        private IMixedRealityDataProviderAccess dataProviderAccess = null;

        private void OnEnable()
        {
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
        }

        private void OnDisable()
        {
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler>(this);
        }


        public Material mat;
        // Start is called before the first frame update
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

            /*
            Mesh mesh = new Mesh();

            // 頂点座標情報
            mesh.vertices = new Vector3[] {
                new Vector3(   0f,  1f, 0f),
                new Vector3( 0.5f,  0f, 0f),
                new Vector3(-0.5f,  0f, 0f),
                new Vector3(   1f, -1f, 0f),
                new Vector3(   0f, -1f, 0f),
                new Vector3(  -1f, -1f, 0f)
            };

            //各頂点にUV座標を設定する
            mesh.uv = new Vector2[] {
                new Vector2 ( 0.5f,   1f),
                new Vector2 (0.75f, 0.5f),
                new Vector2 (0.25f, 0.5f),
                new Vector2 (   1f,   0f),
                new Vector2 ( 0.5f,   0f),
                new Vector2 (   0f,   0f)
            };

            // 頂点を結ぶ順番情報
            mesh.triangles = new int[] {
                0, 1, 2,
                1, 3, 4,
                2, 4, 5
            };

            // 三角形と頂点からメッシュの法線を再計算する
            mesh.RecalculateNormals();

            // バウンディングボリュームを再計算する
            mesh.RecalculateBounds();

            // MeshRendererにメッシュ情報を渡すために必要
            MeshFilter filter = GetComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            filter.sharedMesh.name = "SampleMesh";

            // マテリアルを設定する
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            renderer.material = mat;
            */
        }

        private int count = 0;
        private Vector3[] vertices = new Vector3[3];
        public void OnInputDown(InputEventData eventData)
        {
            // Select と Grip Press のアクションが発火し２回呼ばれるためフィルタ
            if (eventData.MixedRealityInputAction == selectAction)
            {
                //Debug.Log(eventData.InputSource.SourceName);
                //Debug.Log(eventData.InputSource.SourceId);
                //Debug.Log(eventData.MixedRealityInputAction.Id);

                if (eventData.Handedness == Handedness.Left)
                {
                    //Debug.Log("Left");
                }
                else if (eventData.Handedness == Handedness.Right)
                {
                    Debug.Log("Right");

                    // 左手人差し指のポジションを取得する
                    var leftIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left);
                    if (leftIndexTip == null)
                    {
                        Debug.Log("leftIndexTip is null.");
                        return;
                    }
                    Instantiate(prefab, leftIndexTip.position, leftIndexTip.rotation);

                    vertices[count] = leftIndexTip.position;
                    count++;
                    if (count > 2)
                    {
                        count = 0;

                        var go = Instantiate(new GameObject());
                        go.transform.position = Vector3.zero;
                        var filter = go.AddComponent<MeshFilter>();
                        var renderer = go.AddComponent<MeshRenderer>();

                        Mesh mesh = new Mesh();
                        mesh.vertices = vertices;
                        mesh.triangles = new int[] {
                            0, 1, 2,
                            0, 2, 1,
                        };
                        mesh.RecalculateNormals();
                        mesh.RecalculateBounds();
                        // MeshRendererにメッシュ情報を渡すために必要
                        //MeshFilter filter = GetComponent<MeshFilter>();
                        filter.sharedMesh = mesh;
                        filter.sharedMesh.name = "SampleMesh";
                        // マテリアルを設定する
                        //MeshRenderer renderer = GetComponent<MeshRenderer>();
                        renderer = GetComponent<MeshRenderer>();
                        renderer.material = mat;
                    }
                    //Debug.Log(leftIndexTip.position);
                }

                /*
                var camera = Camera.main.transform;
                var pos = camera.position + camera.forward * 2;
                var rot = Quaternion.identity;
                Instantiate(prefab, pos, rot);
                */
            }
        }

        public void OnInputUp(InputEventData eventData)
        {
        }
    }
}

