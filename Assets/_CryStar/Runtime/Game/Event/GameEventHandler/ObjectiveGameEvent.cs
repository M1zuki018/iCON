using CryStar.Game.Attributes;
using CryStar.Game.Enums;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.Game.Events
{
    /// <summary>
    /// Objective - イベントハンドラーの説明
    /// </summary>
    [GameEventHandler(GameEventType.Objective)]
    public class ObjectiveGameEvent : GameEventHandlerBase
    {
        public override GameEventType SupportedGameEventType => GameEventType.Objective;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ObjectiveGameEvent(InGameManager inGameManager) : base(inGameManager) { }
        
        public override UniTask HandleGameEvent(GameEventParameters parameters)
        {
            return UniTask.CompletedTask;
        }
    }
}