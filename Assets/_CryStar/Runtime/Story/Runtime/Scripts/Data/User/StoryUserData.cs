using System;
using System.Collections.Generic;
using CryStar.Data;
using CryStar.Save;
using UnityEngine;

/// <summary>
/// ストーリーのユーザーデータ
/// </summary>
[Serializable]
public class StoryUserData : BaseUserData
{
    [SerializeField] private List<EventClearData> _clearedStories = new List<EventClearData>();
    
    public event Action<int> OnStorySave;
    private Dictionary<int, int> _storyClearCache;

    public StoryUserData(int userId) : base(userId)
    {
        BuildCache();
    }

    /// <summary>
    /// クリアしたか
    /// </summary>
    public void AddStoryClearData(int storyId)
    {
        if (_storyClearCache.ContainsKey(storyId))
        {
            // 既にクリア済みであればreturn
            return;
        }
        
        // シリアライズ用リストを更新
        var existingData = _clearedStories.Find(x => x.EventId == storyId);
        if (existingData != null)
        {
            existingData.ClearCount = _storyClearCache[storyId];
        }
        else
        {
            _clearedStories.Add(new EventClearData(storyId, 1));
        }
        
        _storyClearCache[storyId] = 1;
        OnStorySave?.Invoke(storyId);
    }

    /// <summary>
    /// 前提ストーリーをクリアしているか
    /// </summary>
    public bool IsPremiseStoryClear(int storyId)
    {
        return _storyClearCache.ContainsKey(storyId);
    }

    /// <summary>
    /// パフォーマンス向上のためのキャッシュを構築
    /// </summary>
    private void BuildCache()
    {
        _storyClearCache = new Dictionary<int, int>();
        if (_clearedStories != null)
        {
            foreach (var eventData in _clearedStories)
            {
                _storyClearCache[eventData.EventId] = eventData.ClearCount;
            }
        }
    }
}