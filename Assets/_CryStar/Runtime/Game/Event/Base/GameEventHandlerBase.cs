using CryStar.Game.Data;
using CryStar.Game.Enums;
using CryStar.Story.Factory;
using Cysharp.Threading.Tasks;

namespace CryStar.Game.Events
{
    /// <summary>
    /// ゲームイベントを処理するための基底クラス
    /// NOTE: このクラスを継承することで、新しい種類のイベントを処理するハンドラを定義できます
    /// </summary>
    public abstract class GameEventHandlerBase : IHandlerBase, IGameEventHandler
    {
        /// <summary>
        /// このハンドラが担当するゲームイベントの種類
        /// </summary>
        public abstract GameEventType SupportedGameEventType { get; }

        /// <summary>
        /// ゲームイベントを実行する
        /// NOTE: 継承クラスで具体的な処理を実装する必要があります
        /// </summary>
        public abstract UniTask HandleGameEvent(GameEventData data);
    }
}
