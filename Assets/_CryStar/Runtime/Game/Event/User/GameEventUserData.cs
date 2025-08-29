using System;
using System.Collections.Generic;
using System.Linq;
using CryStar.Data;

/// <summary>
/// ゲームイベントのセーブデータ用クラス
/// </summary>
[Serializable]
public class GameEventUserData : BaseUserData
{
    private Dictionary<int, int> _eventClearData = new Dictionary<int, int>();

    public GameEventUserData(int userId) : base(userId) { }

    /// <summary>
    /// イベントクリアを記録する
    /// </summary>
    public void AddClearCount(int eventId)
    {
        if (!_eventClearData.ContainsKey(eventId))
        {
            _eventClearData.Add(eventId, 0);
        }
        
        _eventClearData[eventId]++;
    }

    public int GetLastClearCount()
    {
        // まだ一つもクリアしていない場合は1を返す
        if (_eventClearData.Count == 0)
        {
            return 1;
        }
        
        // 1から順番に未クリアのイベントを探す
        for (int eventId = 1; eventId < MasterGameEvent.GetGameEventCount() + 1; eventId++)
        {
            if (!_eventClearData.ContainsKey(eventId))
            {
                return eventId;
            }
        }
        
        // 全てのイベントをクリアしている場合は-1を返す
        return -1;
    }
}
