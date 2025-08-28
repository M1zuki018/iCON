using CryStar.Game.Enums;

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
        private string _parameter;

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
        public string Parameter => _parameter;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameEventData(int id, GameEventType eventType, string parameter)
        {
            _id = id;
            _eventType = eventType;
            _parameter = parameter;
        }
    }
}
