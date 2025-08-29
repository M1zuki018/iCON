using System;

/// <summary>
/// セーブスロット情報
/// </summary>
[Serializable]
public class SaveSlotInfo
{
    public int SlotIndex;
    public int UserId;
    public long LastSaveTime;
    public int CurrentMapId;
    public bool IsCurrentSlot;

    /// <summary>
    /// 最後の保存時間を日時形式で取得
    /// </summary>
    public DateTime GetLastSaveDateTime()
    {
        return DateTimeOffset.FromUnixTimeSeconds(LastSaveTime).DateTime;
    }

    /// <summary>
    /// 最後の保存時間を文字列で取得
    /// </summary>
    public string GetLastSaveTimeString()
    {
        return GetLastSaveDateTime().ToString("yyyy/MM/dd HH:mm:ss");
    }
}