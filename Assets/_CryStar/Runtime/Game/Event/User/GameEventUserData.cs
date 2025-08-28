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
    private static Dictionary<int, int> _eventClearData = new Dictionary<int, int>();

    public GameEventUserData(int userId) : base(userId) { }

    /// <summary>
    /// イベントクリアを記録する
    /// </summary>
    public void AddClearCount(int eventId)
    {
        _eventClearData[eventId]++;
    }

    public int GetLastClearCount()
    {
        // まだ一つもクリアしていない場合は1を返す
        if (_eventClearData.Count == 0)
        {
            return 1;
        }
        
        // キーを昇順でソートしてループ
        foreach (int eventId in _eventClearData.Keys.OrderBy(x => x))
        {
            if (_eventClearData[eventId] == 0)
            {
                return eventId;
            }
        }
        
        // 全てクリア済みだった場合は例外として-1を返す
        return -1;
    }
}
