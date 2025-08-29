using System.Threading;
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
    /// PlaySE - SEを再生する
    /// </summary>
    [OrderHandler(OrderType.PlaySE)]
    public class PlaySEOrderHandler : AsyncOrderHandlerBase
    {
        /// <summary>
        /// AudioManager
        /// </summary>
        private AudioManager _audioManager = ServiceLocator.GetGlobal<AudioManager>();
        
        public override OrderType SupportedOrderType => OrderType.PlaySE;
        
        public override async UniTask<Tween> HandleOrderAsync(OrderData data, StoryView view, CancellationToken cancellationToken)
        {
            if (_audioManager == null)
            {
                // 万が一初期化時に取得出来ていなかったら、取得しなおし
                _audioManager = ServiceLocator.GetGlobal<AudioManager>();
            }
            await _audioManager.PlaySE(data.FilePath, data.OverrideTextSpeed);
            return null;
        }
    }
}