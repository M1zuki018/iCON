using System;
using UnityEngine;
using UnityEngine.UI;

namespace iCON.UI
{
    /// <summary>
    /// UIContents ストーリー画面のオーバーレイ上のUIを管理
    /// </summary>
    public class UIContents_OverlayContents : MonoBehaviour
    {
        /// <summary>
        /// UI非表示ボタン
        /// </summary>
        [SerializeField]
        private Button _immersedButton;

        /// <summary>
        /// オート再生ボタン
        /// </summary>
        [SerializeField]
        private Button _autoPlayButton;
        
        /// <summary>
        /// スキップボタン
        /// </summary>
        [SerializeField]
        private Button _skipButton;
        
        /// <summary>
        /// ボタンのデフォルト色
        /// </summary>
        [SerializeField]
        private Color _defaultButtonColor;
        
        #region Lifecycle

        private void OnDestroy()
        {
            _immersedButton.onClick.RemoveAllListeners();
            _autoPlayButton.onClick.RemoveAllListeners();
            _skipButton.onClick.RemoveAllListeners();
        }

        #endregion

        /// <summary>
        /// UI非表示ボタンのセットアップ
        /// </summary>
        public void SetupImmerseButton(Action action)
        {
            // NOTE: 非表示になったときはダイアログを非表示に・ストーリーが進行しないようにする
            _immersedButton.onClick.RemoveAllListeners();
            _immersedButton.onClick.AddListener(() => action?.Invoke());
        }

        /// <summary>
        /// オート再生ボタンのセットアップ
        /// </summary>
        public void SetupAutoPlayButton(Action action)
        {
            _autoPlayButton.onClick.RemoveAllListeners();
            _autoPlayButton.onClick.AddListener(() => action?.Invoke());
        }

        /// <summary>
        /// スキップボタンのセットアップ
        /// </summary>
        /// <param name="action"></param>
        public void SetupSkipButton(Action action)
        {
            _skipButton.onClick.RemoveAllListeners();
            _skipButton.onClick.AddListener(() => action?.Invoke());
        }

        /// <summary>
        /// UI非表示ボタンの色を変更する
        /// </summary>
        public void ChangeImmerseButtonColor(bool isActive)
        {
            _immersedButton.image.color = isActive ? Color.gray : _defaultButtonColor;
        }

        /// <summary>
        /// オート再生ボタンの色を変更する
        /// </summary>
        public void ChangeAutoPlayButtonColor(bool isActive)
        {
            _autoPlayButton.image.color = isActive ? Color.gray : _defaultButtonColor;
        }
        
        #region Private Methods


        #endregion
    }
}