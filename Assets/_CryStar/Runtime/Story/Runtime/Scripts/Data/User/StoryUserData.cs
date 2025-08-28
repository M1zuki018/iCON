using System;
using System.Collections.Generic;
using CryStar.Data;

/// <summary>
/// ストーリーのユーザーデータ
/// </summary>
public class StoryUserData : BaseUserData
{
    public event Action<int> OnStorySave;
    private static Dictionary<int, bool> _storySaveData = new Dictionary<int, bool>();

    public StoryUserData(int userId) : base(userId)
    {
    }

    /// <summary>
    /// クリアしたか
    /// </summary>
    public void AddStoryClearData(int storyId)
    {
        if (_storySaveData.TryGetValue(storyId, out bool isCleared) && isCleared)
        {
            // 既にクリア済みであればreturn
            return;
        }
        
        _storySaveData[storyId] = true;
        OnStorySave?.Invoke(storyId);
    }

    /// <summary>
    /// 前提ストーリーをクリアしているか
    /// </summary>
    public bool IsPremiseStoryClear(int storyId)
    {
        return _storySaveData.ContainsKey(storyId);
    }
}