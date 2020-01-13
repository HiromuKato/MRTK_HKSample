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
        private TouchScreenKeyboard keyboard = null;

        [SerializeField]
        private TextMesh inputText;

        void Start()
        {
            inputText.text = "";
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
                    keyboard = null;
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
                        keyboard = null;
                    }
                }
            }
        }

        /// <summary>
        /// キーボードを表示する
        /// </summary>
        public void ShowKeyboard()
        {
            if (keyboard != null)
            {
                Debug.Log("keyboard : not null");
                keyboard.active = false;
                keyboard = null;
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
                keyboard = null;
            }
        }
    }
}
