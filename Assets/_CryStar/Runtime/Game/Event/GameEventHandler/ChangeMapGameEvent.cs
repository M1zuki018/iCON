using CryStar.Game.Attributes;
using CryStar.Game.Enums;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.Game.Events
{
    /// <summary>
    /// ChangeMap - イベントハンドラーの説明
    /// </summary>
    [GameEventHandler(GameEventType.ChangeMap)]
    public class ChangeMapGameEvent : GameEventHandlerBase
    {
        public override GameEventType SupportedGameEventType => GameEventType.ChangeMap;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChangeMapGameEvent(InGameManager inGameManager) : base(inGameManager) { }
        
        public override UniTask HandleGameEvent(GameEventParameters parameters)
        {
            return UniTask.CompletedTask;
        }
    }
}