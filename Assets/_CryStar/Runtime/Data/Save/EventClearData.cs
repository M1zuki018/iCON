using System;

namespace CryStar.Data
{
    /// <summary>
    /// Json保存用のイベントデータ
    /// NOTE: 実行時はDictionaryに変換して使う
    /// </summary>
    [Serializable]
    public class EventClearData : IEquatable<EventClearData>
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

        /// <summary>
        /// 等しいかどうか判定
        /// </summary>
        public bool Equals(EventClearData other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return EventId == other.EventId && ClearCount == other.ClearCount;
        }
        
        /// <summary>
        /// 文字列表現を返す
        /// </summary>
        public override string ToString()
        {
            return $"EventClearData(EventId: {EventId}, ClearCount: {ClearCount})";
        }
    }
}
