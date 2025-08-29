using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryStar.Data.User
{
    /// <summary>
    /// キャッシュ機能をもつユーザーデータのクラス
    /// </summary>
    [Serializable]
    public abstract class CachedUserDataBase : UserDataBase
    {
        [SerializeField] protected List<EventClearData> _clearedDataList = new List<EventClearData>();

        protected Dictionary<int, int> _clearedDataCache = new Dictionary<int, int>();
        
        /// <summary>
        /// セーブデータのリストのプロパティ（復元用）
        /// </summary>
        public List<EventClearData> ClearedDataList => _clearedDataList;
        
        /// <summary>
        /// EventClearDataのキャッシュ
        /// </summary>
        public IReadOnlyDictionary<int, int> ClearedDataCache => _clearedDataCache;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected CachedUserDataBase(int userId) : base(userId) { }
        
        /// <summary>
        /// データ復元用
        /// </summary>
        public void SetClearedData(List<EventClearData> clearedData)
        {
            if (_clearedDataList == null)
            {
                _clearedDataList = new List<EventClearData>();
            }
            
            _clearedDataList.Clear();
            _clearedDataList = clearedData;
            
            // 実行時用のDictionaryを構築する
            BuildCache();
        }

        /// <summary>
        /// クリアデータを更新
        /// </summary>
        public virtual void AddClearData(int eventId)
        {
            if (_clearedDataCache.ContainsKey(eventId))
            {
                _clearedDataCache[eventId]++;
            }
            else
            {
                _clearedDataCache[eventId] = 1;
            }
            
            UpdateSaveDataList(eventId);
        }
        
        /// <summary>
        /// 前提ストーリーをクリアしているか
        /// </summary>
        public bool IsPremiseStoryClear(int storyId)
        {
            return _clearedDataCache.ContainsKey(storyId);
        }
        
        /// <summary>
        /// セーブデータを更新する
        /// </summary>
        protected void UpdateSaveDataList(int eventId)
        {
            // シリアライズ用リストを更新
            var existingData = _clearedDataList.Find(x => x.EventId == eventId);
            if (existingData != null)
            {
                existingData.ClearCount = _clearedDataCache[eventId];
            }
            else
            {
                _clearedDataList.Add(new EventClearData(eventId, 1));
            }
        }
        
        /// <summary>
        /// パフォーマンス向上のためのキャッシュを構築
        /// </summary>
        protected virtual void BuildCache()
        {
            _clearedDataCache = new Dictionary<int, int>();
            if (_clearedDataList != null)
            {
                foreach (var eventData in _clearedDataList)
                {
                    _clearedDataCache[eventData.EventId] = eventData.ClearCount;
                }
            }
        }
    }
}