using System;
using System.Collections.Generic;
using System.Linq;
using CryStar.Data;
using CryStar.Save;
using UnityEngine;

/// <summary>
/// ゲームイベントのセーブデータ用クラス
/// </summary>
[Serializable]
public class GameEventUserData : BaseUserData
{
    [SerializeField] private List<EventClearData> _clearedEvents = new List<EventClearData>();
    
    private Dictionary<int, int> _eventClearCache = new Dictionary<int, int>();
    
    public List<EventClearData> ClearedEvents => _clearedEvents;

    public GameEventUserData(int userId) : base(userId) { }

    /// <summary>
    /// セーブデータの復元用
    /// </summary>
    public void SetClearedEvents(List<EventClearData> events)
    {
        _clearedEvents.Clear();
        _clearedEvents = events;
        
        // 実行時用のキャッシュを構築
        BuildCache();
    }
    
    /// <summary>
    /// イベントクリアを記録する
    /// </summary>
    public void AddClearCount(int eventId)
    {
        if (!_eventClearCache.ContainsKey(eventId))
        {
            _eventClearCache.Add(eventId, 0);
        }
        
        _eventClearCache[eventId]++;
        
        // シリアライズ用リストを更新
        var existingData = _clearedEvents.Find(x => x.EventId == eventId);
        if (existingData != null)
        {
            existingData.ClearCount = _eventClearCache[eventId];
        }
        else
        {
            _clearedEvents.Add(new EventClearData(eventId, 1));
        }
    }

    public int GetLastClearCount()
    {
        // まだ一つもクリアしていない場合は1を返す
        if (_eventClearCache.Count == 0)
        {
            return 1;
        }
        
        // 1から順番に未クリアのイベントを探す
        for (int eventId = 1; eventId < MasterGameEvent.GetGameEventCount() + 1; eventId++)
        {
            if (!_eventClearCache.ContainsKey(eventId))
            {
                return eventId;
            }
        }
        
        // 全てのイベントをクリアしている場合は-1を返す
        return -1;
    }
    
    /// <summary>
    /// パフォーマンス向上のためのキャッシュを構築
    /// </summary>
    private void BuildCache()
    {
        _eventClearCache = new Dictionary<int, int>();
        if (_clearedEvents != null)
        {
            foreach (var eventData in _clearedEvents)
            {
                _eventClearCache[eventData.EventId] = eventData.ClearCount;
            }
        }
    }
}
