using System.Collections.Generic;
using CryStar.Data;

/// <summary>
/// ストーリーのユーザーデータ
/// </summary>
public class StoryUserData : BaseUserData
{
    private static Dictionary<StorySaveData, bool> _storySaveData = new Dictionary<StorySaveData, bool>();

    public StoryUserData(int userId) : base(userId)
    {
    }

    /// <summary>
    /// クリアしたか
    /// </summary>
    public static void AddStoryClearData(StorySaveData storySaveData)
    {
        _storySaveData[storySaveData] = true;
    }

    /// <summary>
    /// 前提ストーリーをClearしているか
    /// </summary>
    /// <returns></returns>
    public static bool IsPremiseStoryClear(StorySaveData storySaveData)
    {
        return _storySaveData.ContainsKey(storySaveData);
    }
}