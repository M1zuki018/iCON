using CryStar.Game.Enums;
using CryStar.Game.Events;

namespace CryStar.Game.Data
{
    /// <summary>
    /// ゲームイベントデータ
    /// </summary>
    public class GameEventData
    {
        #region Private Field

        private int _id;
        private GameEventType _eventType;
        private GameEventParameters _parameter;

        #endregion

        /// <summary>
        /// ゲームイベントのID
        /// </summary>
        public int Id => _id;
        
        /// <summary>
        /// ゲームイベントの種類
        /// </summary>
        public GameEventType EventType => _eventType;
        
        /// <summary>
        /// 入力されたパラメーター
        /// </summary>
        public GameEventParameters Parameter => _parameter;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameEventData(int id, GameEventType eventType, GameEventParameters parameter)
        {
            _id = id;
            _eventType = eventType;
            _parameter = parameter;
        }
    }
}
