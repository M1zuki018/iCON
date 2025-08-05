using System;
using CryStar.Attribute;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace iCON.UI
{
    /// <summary>
    /// CanvasController_Title
    /// </summary>
    public partial class CanvasController_Title : WindowBase
    {
        [SerializeField, HighlightIfNull] private CustomButton _startButton;
        
        public event Action OnStartButtonClicked;
        
        public override UniTask OnAwake()
        {
            // イベント登録
            if (_startButton != null)
            {
                _startButton.onClick.AddListener(HandleStartButtonClicked);
                _startButton.enabled = true;
            }
            return base.OnAwake();
        }

        private void HandleStartButtonClicked()
        {
            if (_startButton != null)
            {
                _startButton.enabled = false;
                
                // 押したことが分かりやすいように色を変更
                _startButton.image.color = Color.cyan;
            }
            
            OnStartButtonClicked?.Invoke();
        }

        private void OnDestroy()
        {
            if(_startButton != null) _startButton.onClick?.RemoveAllListeners();
        }
    }
}