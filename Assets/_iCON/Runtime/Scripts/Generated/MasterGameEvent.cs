using System.Collections.Generic;
using CryStar.Game.Data;
using CryStar.Game.Enums;
using CryStar.Game.Events;
using UnityEngine;

public static class MasterGameEvent
{
    private static readonly Dictionary<int, GameEventSequenceData> _eventData = new Dictionary<int, GameEventSequenceData>()
    {
        {
            1, new GameEventSequenceData(
                new GameEventExecutionData(ExecutionType.Sequential, 
                    new GameEventData[]
                    {
                        new(GameEventType.PlayStory, new GameEventParameters(intParam: 1)),
                    }),
                new GameEventExecutionData(ExecutionType.Sequential,
                    new GameEventData[]
                    {
                        new(GameEventType.EventTransition, new GameEventParameters(intParam: 2)),
                    }))
        },
        {
            2, new GameEventSequenceData(
                new GameEventExecutionData(ExecutionType.Sequential, 
                    new GameEventData[]
                    {
                        new(GameEventType.PlayStory, new GameEventParameters(intParam: 2)),
                    }),
                new GameEventExecutionData(ExecutionType.Sequential,
                    new GameEventData[]
                    {
                        new(GameEventType.EventTransition, new GameEventParameters(intParam: 3)),
                    }))
        },
        {
            3, new GameEventSequenceData(
                new GameEventExecutionData(ExecutionType.Parallel, 
                    new GameEventData[]
                    {
                        new(GameEventType.Objective, new GameEventParameters(stringParam: "衣装スタッフに声をかける")),
                        new(GameEventType.StoryPreload, new GameEventParameters(intArrayParam: new []{3,4,5})),
                    }))
        },
        {
            4, new GameEventSequenceData(
                new GameEventExecutionData(ExecutionType.Sequential, 
                    new GameEventData[]
                    {
                        new(GameEventType.PlayStory, new GameEventParameters(intParam: 3)),
                    }))
        },
        {
            7, new GameEventSequenceData(
                new GameEventExecutionData(ExecutionType.Sequential, 
                    new GameEventData[]
                    {
                        new(GameEventType.PlayStory, new GameEventParameters(intParam: 5)),
                    }),
                new GameEventExecutionData(ExecutionType.Sequential, 
                    new GameEventData[]
                    {
                        new(GameEventType.ChangeMap, new GameEventParameters(intParam: 3)),
                    }))
        },
        {
            8, new GameEventSequenceData(
                new GameEventExecutionData(ExecutionType.Sequential, 
                    new GameEventData[]
                    {
                        new(GameEventType.PlayStory, new GameEventParameters(intParam: 6)),
                    }),
                new GameEventExecutionData(ExecutionType.Sequential,
                    new GameEventData[]
                    {
                        new(GameEventType.BattleStart),
                    }))
        },
        {
            9, new GameEventSequenceData(
                new GameEventExecutionData(ExecutionType.Sequential, 
                    new GameEventData[]
                    {
                        new(GameEventType.PlayStory, new GameEventParameters(intParam: 7)),
                    }),
                new GameEventExecutionData(ExecutionType.Sequential,
                    new GameEventData[]
                    {
                        new(GameEventType.GameClear),
                    }))
        }
    };
    
    /// <summary>
    /// ゲームイベント実行データを取得する
    /// </summary>
    public static GameEventSequenceData GetGameEventSequenceData(int eventId)
    {
        return _eventData[eventId];
    } 
}
