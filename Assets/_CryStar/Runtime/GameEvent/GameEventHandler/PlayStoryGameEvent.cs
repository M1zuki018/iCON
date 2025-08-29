using CryStar.Game.Enums;
using CryStar.GameEvent;
using CryStar.GameEvent.Attributes;
using CryStar.GameEvent.Data;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.Game.Events
{
    /// <summary>
    /// PlayStory - ストーリー再生
    /// </summary>
    [GameEventHandler(GameEventType.PlayStory)]
    public class PlayStoryGameEvent : GameEventHandlerBase
    {
        public override GameEventType SupportedGameEventType => GameEventType.PlayStory;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PlayStoryGameEvent(InGameManager inGameManager) : base(inGameManager) { }
        
        /// <summary>
        /// ストーリーを再生する
        /// </summary>
        public override async UniTask HandleGameEvent(GameEventParameters parameters)
        {
            await InGameManager.PlayStory(parameters.IntParam);
        }
    }
}