using CryStar.Game.Attributes;
using CryStar.Game.Enums;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.Game.Events
{
    /// <summary>
    /// Custom - イベントハンドラーの説明
    /// </summary>
    [GameEventHandler(GameEventType.Custom)]
    public class CustomGameEvent : GameEventHandlerBase
    {
        public override GameEventType SupportedGameEventType => GameEventType.Custom;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomGameEvent(InGameManager inGameManager) : base(inGameManager) { }
        
        public override UniTask HandleGameEvent(GameEventParameters parameters)
        {
            return UniTask.CompletedTask;
        }
    }
}