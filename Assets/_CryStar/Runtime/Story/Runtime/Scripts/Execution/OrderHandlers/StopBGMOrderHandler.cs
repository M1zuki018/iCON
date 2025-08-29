using CryStar.Audio;
using CryStar.Core;
using CryStar.Story.Attributes;
using CryStar.Story.Data;
using CryStar.Story.Enums;
using CryStar.Story.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace CryStar.Story.Execution
{
    /// <summary>
    /// StopBGM - BGM再生を止める
    /// </summary>
    [OrderHandler(OrderType.StopBGM)]
    public class StopBGMOrderHandler : OrderHandlerBase
    {
        /// <summary>
        /// AudioManager
        /// </summary>
        private AudioManager _audioManager = ServiceLocator.GetGlobal<AudioManager>();
        public override OrderType SupportedOrderType => OrderType.StopBGM;
        
        public override Tween HandleOrder(OrderData data, StoryView view)
        {
            if (_audioManager == null)
            {
                // 万が一初期化時に取得出来ていなかったら、取得しなおし
                _audioManager = ServiceLocator.GetGlobal<AudioManager>();
            }
            _audioManager.FadeOutBGM(data.Duration).Forget();
            return null;
        }
    }
}