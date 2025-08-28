using CryStar.Game.Enums;

namespace CryStar.Game.Events
{
    /// <summary>
    /// ゲームイベント実行に必要なゲームイベントデータの最小単位
    /// </summary>
    public class GameEventData
    {
        private GameEventType _eventType;
        private GameEventParameters _parameters;
        
        /// <summary>
        /// ゲームイベントの種類
        /// </summary>
        public GameEventType EventType => _eventType;
        
        /// <summary>
        /// パラメーター
        /// </summary>
        public GameEventParameters Parameters => _parameters;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameEventData(GameEventType eventType, GameEventParameters parameters)
        {
            _eventType = eventType;
            _parameters = parameters;
        }
    }
}