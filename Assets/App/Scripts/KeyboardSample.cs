using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRTK_HKSample
{
    /// <summary>
    /// ソフトキーボードのサンプル（実機でしか動作しないので注意）
    /// </summary>
    public class KeyboardSample : MonoBehaviour
    {
        private TouchScreenKeyboard keyboard;

        [SerializeField]
        private TextMesh inputText;

        [SerializeField]
        private TextMesh debugText;

        void Start()
        {
            inputText.text = "";
            debugText.text = "";
        }

        void Update()
        {
            if (keyboard != null)
            {
                // キーボードを閉じたとき
                if (keyboard.status == TouchScreenKeyboard.Status.Done ||
                    keyboard.status == TouchScreenKeyboard.Status.Canceled)
                {
                    Debug.Log("keyboard : Done or Cancelled");
                    if (inputText != null)
                    {
                        inputText.text = keyboard.text;
                    }
                    keyboard.active = false;
                }
                else
                {
                    if (inputText != null)
                    {
                        inputText.text = keyboard.text;
                    }
                    if (keyboard.status != TouchScreenKeyboard.Status.Visible)
                    {
                        Debug.Log("keyboard : Not Visible");
                        keyboard.active = false;
                    }
                }
            }
        }

        /// <summary>
        /// キーボードを表示する
        /// </summary>
        public void ShowKeyboard()
        {
            // キーボード表示中はボタンが反応しないようにしたいが、Unityのバグで現状キーボードの
            // 正しい表示状態が取得できない
            // https://issuetracker.unity3d.com/issues/hololens-touchscreenkeyboard-doesnt-report-correct-status-or-active-values
            /*
            if (TouchScreenKeyboard.visible)
            {
                return;
            }
            */

            if (keyboard != null)
            {
                Debug.Log("keyboard : not null");
                keyboard.active = false;
            }

            keyboard = TouchScreenKeyboard.Open(inputText.text, TouchScreenKeyboardType.EmailAddress);
            keyboard.active = true;
            keyboard.text = inputText.text;
        }

        /// <summary>
        /// キーボードを消す
        /// </summary>
        public void HideKeyboard()
        {
            if (keyboard != null)
            {
                keyboard.active = false;
            }
        }

        /// <summary>
        /// キーボードの状態を表示する
        /// </summary>
        public void ShowKeyboardStatus()
        {
            debugText.text = "visible:" + TouchScreenKeyboard.visible.ToString();

            if(keyboard != null)
            {
                debugText.text += "\nactive:" + keyboard.active + "\nstatus:" + keyboard.status;
            }
            else
            {
                debugText.text += "\nKeyboard is null";
            }
        }

    }
}
