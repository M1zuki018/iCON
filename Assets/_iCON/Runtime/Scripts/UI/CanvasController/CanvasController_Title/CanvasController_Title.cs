using System;
using CryStar.Attribute;
using CryStar.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace iCON.UI
{
    /// <summary>
    /// CanvasController_Title
    /// </summary>
    public partial class CanvasController_Title : WindowBase
    {
        /// <summary>
        ///  スタートボタンを押した時のコールバック
        /// </summary>
        public event Action OnStartButtonClicked;
        
        [SerializeField, HighlightIfNull] private CustomButton _startButton;
        
        public override UniTask OnAwake()
        {
            // イベント登録
            _startButton.onClick.SafeAddListener(HandleStartButtonClicked);

            if (_startButton != null)
            {
                // 確実に有効化しておく
                _startButton.enabled = true;
            }
           
            return base.OnAwake();
        }

        /// <summary>
        /// スタートボタンを押したときの処理
        /// </summary>
        private void HandleStartButtonClicked()
        {
            if (_startButton != null)
            {
                // 二度押しされないようにボタン無効化
                _startButton.enabled = false;
                
                // 押したことが分かりやすいように色を変更
                _startButton.image.color = Color.cyan;
            }
            
            OnStartButtonClicked?.Invoke();
        }

        private void OnDestroy()
        {
            _startButton.onClick?.SafeRemoveAllListeners();
        }
    }
}