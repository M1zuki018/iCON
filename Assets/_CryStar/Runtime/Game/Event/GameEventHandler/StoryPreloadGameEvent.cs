using CryStar.Game.Attributes;
using CryStar.Game.Enums;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.Game.Events
{
    /// <summary>
    /// StoryPreload - イベントハンドラーの説明
    /// </summary>
    [GameEventHandler(GameEventType.StoryPreload)]
    public class StoryPreloadGameEvent : GameEventHandlerBase
    {
        public override GameEventType SupportedGameEventType => GameEventType.StoryPreload;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StoryPreloadGameEvent(InGameManager inGameManager) : base(inGameManager) { }
        
        public override UniTask HandleGameEvent(GameEventParameters parameters)
        {
            return UniTask.CompletedTask;
        }
    }
}