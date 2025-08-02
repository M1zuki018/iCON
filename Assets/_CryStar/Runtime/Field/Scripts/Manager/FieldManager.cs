using CryStar.Attribute;
using CryStar.Core;
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

        // TODO: UI管理もこのクラスから行う
        
        public override async UniTask OnAwake()
        {
            // TODO: 仮実装
            ShowMap(1);
            await base.OnAwake();
        }
        
        /// <summary>
        /// 指定したマップを表示状態にする
        /// </summary>
        public void ShowMap(int mapId)
        {
            _mapInstanceManager.ShowMap(mapId);
        }
    }
}