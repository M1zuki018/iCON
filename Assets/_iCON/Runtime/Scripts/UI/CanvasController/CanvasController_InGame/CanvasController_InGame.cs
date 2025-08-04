using System;
using System.Threading;
using CryStar.Attribute;
using CryStar.Utility;
using CryStar.Utility.Enum;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace iCON.UI
{
    /// <summary>
    /// CanvasController_InGame
    /// </summary>
    public partial class CanvasController_InGame : WindowBase
    {
        [Header("目標表示用の設定")]
        [SerializeField, HighlightIfNull] private CustomText _objectiveText;
        [SerializeField] private float _displayTime = 3f;
        private CancellationTokenSource _cts = new CancellationTokenSource();
                
        public override UniTask OnAwake()
        {
            if (_objectiveText == null)
            {
                LogUtility.Error("目標表示用のテキストコンポーネントがアサインされていません", LogCategory.UI, this);
                return base.OnAwake();   
            }

            _objectiveText.enabled = false;
            
            return base.OnAwake();
        }

        private void OnDestroy()
        {
            // キャンセレーショントークンの解放処理
            CancelCurrentOperation();
        }

        /// <summary>
        /// 目標表示を行う
        /// </summary>
        public async UniTask ShowObjectiveText(string message)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            
            try
            {
                _objectiveText.enabled = true;
                // テキストを更新
                _objectiveText.SetText(message);

                // キャンセル可能
                await UniTask.Delay(TimeSpan.FromSeconds(_displayTime), cancellationToken: _cts.Token);

                _objectiveText.enabled = false;
            }
            catch (Exception ex)
            {
                // 連続で目標表示が行われた場合にキャンセル処理が行われる
                // 正常な動作なので、特にログなどは出さない
            }
            
        }

        /// <summary>
        /// 現在の操作をキャンセル
        /// </summary>
        private void CancelCurrentOperation()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}