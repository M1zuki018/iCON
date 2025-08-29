using System;

namespace CryStar.Save
{
    /// <summary>
    /// Json保存用のイベントデータ
    /// </summary>
    [Serializable]
    public class EventClearData
    {
        /// <summary>
        /// イベントID
        /// </summary>
        public int EventId;
        
        /// <summary>
        /// クリア回数
        /// </summary>
        public int ClearCount;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventClearData(int eventId, int clearCount)
        {
            EventId = eventId;
            ClearCount = clearCount;
        }
    }
}
