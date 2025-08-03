using CryStar.Core;
using CryStar.Field.Manager;
using CryStar.Story.Attributes;
using CryStar.Story.Data;
using CryStar.Story.Enums;
using CryStar.Story.UI;
using DG.Tweening;

namespace CryStar.Story.Execution
{
    /// <summary>
    /// ChangeMap - マップ移動
    /// </summary>
    [OrderHandler(OrderType.ChangeMap)]
    public class ChangeMapOrderHandler : OrderHandlerBase
    {
        /// <summary>
        /// フィールドの管理クラス
        /// </summary>
        private FieldManager _mapInstanceManager = ServiceLocator.GetLocal<FieldManager>();
        
        public override OrderType SupportedOrderType => OrderType.ChangeMap;

        public override Tween HandleOrder(OrderData data, StoryView view)
        {
            if (_mapInstanceManager == null)
            {
                // FieldManagerがnullの場合、もう一度サービスロケーターから取得を試す
                _mapInstanceManager = ServiceLocator.GetLocal<FieldManager>();
            }
            
            // マップを移動する
            _mapInstanceManager.ShowMapAndRemove((int)data.OverrideTextSpeed);
            return null;
        }
    }
}
