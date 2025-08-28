using CryStar.Game.Attributes;
using CryStar.Game.Enums;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.Game.Events
{
    /// <summary>
    /// PlayStory - イベントハンドラーの説明
    /// </summary>
    [GameEventHandler(GameEventType.PlayStory)]
    public class PlayStoryGameEvent : GameEventHandlerBase
    {
        public override GameEventType SupportedGameEventType => GameEventType.PlayStory;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PlayStoryGameEvent(InGameManager inGameManager) : base(inGameManager) { }
        
        public override UniTask HandleGameEvent(GameEventParameters parameters)
        {
            return UniTask.CompletedTask;
        }
    }
}