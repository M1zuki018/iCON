using CryStar.Game.Data;
using CryStar.Game.Enums;
using Cysharp.Threading.Tasks;

namespace CryStar.Game.Events
{
    /// <summary>
    /// GameEventHandlerが継承すべきインターフェース
    /// </summary>
    public interface IGameEventHandler
    {
        /// <summary>
        /// このハンドラが担当するゲームイベントの種類
        /// </summary>
        GameEventType SupportedGameEventType { get; }

        /// <summary>
        /// ゲームイベントを実行する
        /// </summary>
        UniTask HandleGameEvent(GameEventData data);
    }
}
