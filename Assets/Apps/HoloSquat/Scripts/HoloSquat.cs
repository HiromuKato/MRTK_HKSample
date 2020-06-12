using Microsoft.MixedReality.Toolkit.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MRTK_HKSample
{
    /// <summary>
    /// スクワットの回数をカウントする
    /// </summary>
    public class HoloSquat : MonoBehaviour
    {
        [SerializeField] private Slider gage;

        [SerializeField] private TextMeshPro pos;
        [SerializeField] private TextMeshPro countLabel;
        private float count = 0;

        [SerializeField] private AudioSource audioSrc;
        [SerializeField] private AudioClip clipDown;
        [SerializeField] private AudioClip clipUp;

        [SerializeField]
        private GameObject effectPrefab = default;
        private ParticleSystem particle;

        private float baseHeadPosY = 0;

        // スクワットしたとみなす閾値（頭を下げる必要のある距離）
        private readonly float threshold = 0.3f;

        private bool isSquatted = false;

        // Start is called before the first frame update
        void Start()
        {
            baseHeadPosY = CameraCache.Main.transform.position.y;

            var particleObj = Instantiate(effectPrefab);
            particleObj.transform.position = new Vector3(0, 0, 2);
            particle = particleObj.GetComponent<ParticleSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            var headPosY = CameraCache.Main.transform.position.y;
            pos.text = "Y pos : " + headPosY.ToString("0.00");

            // ゲージ更新
            gage.value = Mathf.Abs(headPosY) / threshold;
            if (headPosY > 0)
            {
                gage.value = 0;
            }
            else if (headPosY < -threshold)
            {
                gage.value = 1;
            }

            // スクワット処理
            if (isSquatted == false && headPosY < -threshold)
            {
                isSquatted = true;
                audioSrc.PlayOneShot(clipDown);
            }
            if (isSquatted == true && headPosY > 0)
            {
                isSquatted = false;

                count++;
                countLabel.text = count.ToString();

                audioSrc.PlayOneShot(clipUp);

                // パーティクルを生成
                particle.Emit(100);
            }
        }

    }
}
