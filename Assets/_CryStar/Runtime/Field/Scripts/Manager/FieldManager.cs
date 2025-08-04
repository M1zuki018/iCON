using CryStar.Attribute;
using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Field.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CryStar.Field.Manager
{
    /// <summary>
    /// Field Manager
    /// </summary>
    public class FieldManager : CustomBehaviour
    {
        /// <summary>
        /// マップ表示/非表示などの処理を行うクラス
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private MapInstanceManager _mapInstanceManager;

        /// <summary>
        /// View
        /// </summary>
        [SerializeField, HighlightIfNull]
        private FieldView _view;
        
        public override async UniTask OnAwake()
        {
            ServiceLocator.Register(this, ServiceType.Local);
            
            // TODO: 仮実装
            ShowMapAndDisable(1);
            await base.OnAwake();
        }
        
        /// <summary>
        /// 現在表示中のマップを非表示にしたあと指定したマップを表示状態にする
        /// </summary>
        public void ShowMapAndDisable(int mapId)
        {
            _mapInstanceManager.DisableMap(_mapInstanceManager.CurrentMapId);
            _mapInstanceManager.ShowMap(mapId);
        }

        /// <summary>
        /// 現在表示中のマップのインスタンスを削除したあと指定したマップを表示状態にする
        /// </summary>
        public void ShowMapAndRemove(int mapId)
        {
            _mapInstanceManager.RemoveMap(_mapInstanceManager.CurrentMapId);
            _mapInstanceManager.ShowMap(mapId);
        }
        
        /// <summary>
        /// 目標UIを表示する
        /// </summary>
        public async UniTask ShowObjectiveTewt(string message)
        {
            await _view.ShowObjectiveText(message);
        }
    }
}