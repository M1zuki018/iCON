using System.Collections.Generic;
using CryStar.Game.Events;

public static class MasterStoryTriggerEvent
{
    private static Dictionary<int, StoryTriggerEventData> _triggerEventDataList = new Dictionary<int, StoryTriggerEventData>()
    {
        { 6, new StoryTriggerEventData(4, 0)}
    };

    /// <summary>
    /// 辞書の中にイベントをトリガーすべきものがないか検索する
    /// </summary>
    public static StoryTriggerEventData GetTriggerEventData(int storyId)
    {
        if (_triggerEventDataList.TryGetValue(storyId, out var data))
        {
            return data;
        }
        
        return null;
    } 
}
