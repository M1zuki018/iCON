using CryStar.Game.Data;

namespace CryStar.Game.Events
{
    /// <summary>
    /// ゲームイベントの開始時のイベントと終了時のイベントを持った最大のデータ
    /// </summary>
    public class GameEventSequenceData
    {
        private GameEventExecutionData _startEvent;
        private GameEventExecutionData _endEvent;
        
        /// <summary>
        /// 開始時のイベント
        /// </summary>
        public GameEventExecutionData StartEvent => _startEvent;
        
        /// <summary>
        /// 終了時のイベント
        /// 終了時のイベントがない場合はnullが渡される
        /// </summary>
        public GameEventExecutionData EndEvent => _endEvent;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameEventSequenceData(GameEventExecutionData startEvent, GameEventExecutionData endEvent = null)
        {
            _startEvent = startEvent;
            _endEvent = endEvent;
        }
    }
}
