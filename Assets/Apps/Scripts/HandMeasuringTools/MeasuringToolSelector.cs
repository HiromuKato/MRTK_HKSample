using System.Collections.Generic;
using UnityEngine;

namespace HKT
{
    /// <summary>
    /// ハンドメジャーツールを切り替えるためのクラス
    /// 参考動画：https://twitter.com/hi_rom_/status/1267544392962699264
    /// </summary>
    public class MeasuringToolSelector : MonoBehaviour
    {
        public enum MeasuringTool
        {
            OneHandRuler = 0,
            TwoHandsRuler,
            HandProtractor
        }

        [SerializeField]
        private List<GameObject> tools = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            Initialise();
        }

        private void Initialise()
        {
            UseOneHandRuler();
        }

        public void UseOneHandRuler()
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[(int) MeasuringTool.OneHandRuler].SetActive(true);
        }

        public void UseTwoHandsRuler()
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[(int) MeasuringTool.TwoHandsRuler].SetActive(true);
        }

        public void UseHandProtractor()
        {
            foreach (var tool in tools)
            {
                tool.SetActive(false);
            }
            tools[(int) MeasuringTool.HandProtractor].SetActive(true);
        }
    }
}
