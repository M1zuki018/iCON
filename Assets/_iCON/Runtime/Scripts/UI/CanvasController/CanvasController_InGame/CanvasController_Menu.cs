using System;
using CryStar.Attribute;
using CryStar.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace iCON.UI
{
    /// <summary>
    /// CanvasController_InGame
    /// </summary>
    public partial class CanvasController_Menu : WindowBase
    {
        [SerializeField, HighlightIfNull] private CustomButton _statusButton;
        [SerializeField, HighlightIfNull] private CustomButton _itemButton;
        [SerializeField, HighlightIfNull] private CustomButton _backTitleButton;
        
        public event Action OnStatusButtonClicked;
        public event Action OnItemButtonClicked;
        public event Action OnBackTitleButtonClicked;

        public override async UniTask OnUIInitialize()
        {
            await base.OnUIInitialize();
            
            _statusButton.onClick.SafeReplaceListener(HandleStatusButtonClicked);
            _itemButton.onClick.SafeReplaceListener(HandleItemButtonClicked);
            _backTitleButton.onClick.SafeReplaceListener(HandleBackTitleButtonClicked);
        }

        private void OnDestroy()
        {
            Cleanup();
        }
        
        /// <summary>
        /// Cleanup
        /// </summary>
        private void Cleanup()
        {
            _statusButton.onClick.SafeRemoveAllListeners();
            _itemButton.onClick.SafeRemoveAllListeners();
            _backTitleButton.onClick.SafeRemoveAllListeners();
        }

        /// <summary>
        /// キャラクターステータスボタンを押したときの処理
        /// </summary>
        private void HandleStatusButtonClicked() => OnStatusButtonClicked?.Invoke();

        /// <summary>
        /// アイテムボタンを押したときの処理
        /// </summary>
        private void HandleItemButtonClicked() => OnItemButtonClicked?.Invoke();

        /// <summary>
        /// タイトルに戻るボタンを押したときの処理
        /// </summary>
        private void HandleBackTitleButtonClicked() => OnBackTitleButtonClicked?.Invoke(); 
    }
}