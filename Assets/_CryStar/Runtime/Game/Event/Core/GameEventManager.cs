using System;
using System.Collections.Generic;
using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Game.Data;
using CryStar.Game.Enums;
using CryStar.Game.Events.Initialization;
using CryStar.Utility;
using CryStar.Utility.Enum;
using Cysharp.Threading.Tasks;
using iCON.System;

namespace CryStar.Game.Events
{
    /// <summary>
    /// Game Event Manager
    /// </summary>
    public class GameEventManager : CustomBehaviour
    {
        /// <summary>
        /// 各ゲームイベントの列挙型と処理を行うHandlerのインスタンスのkvp
        /// </summary>
        private Dictionary<GameEventType, GameEventHandlerBase> _handlers = new Dictionary<GameEventType, GameEventHandlerBase>();

        /// <summary>
        /// イベントIDとそのIDに対応したイベントデータのkvp
        /// </summary>
        private Dictionary<int, GameEventSequenceData> _eventData = new Dictionary<int, GameEventSequenceData>();
        
        /// <summary>
        /// Awake
        /// </summary>
        public override async UniTask OnBind()
        {
            await base.OnBind();
            
            // インゲーム・バトルシーンで利用するためGlobalサービスに登録
            ServiceLocator.Register(this, ServiceType.Global);
            
            // 念のためゲームイベントシステムが初期化されていることを確認する
            GameEventInitializer.Initialize();
            _handlers = GameEventFactory.CreateAllHandlers(ServiceLocator.GetLocal<InGameManager>());
        }

        /// <summary>
        /// イベントIDを元にイベントを実行する
        /// </summary>
        public async UniTask PlayEvent(int eventID)
        {
            var sequenceData = _eventData[eventID];
            
            // イベント開始時に登録されている処理を実行
            await Execute(sequenceData.StartEvent);
            
            // 終わったらイベント終了時に登録されている処理を実行
            await Execute(sequenceData.EndEvent);
        }

        /// <summary>
        /// イベント実行
        /// </summary>
        /// <param name="eventData"></param>
        private async UniTask Execute(GameEventExecutionData eventData)
        {
            switch (eventData.ExecutionType)
            {
                // 順次実行
                case ExecutionType.Sequential:
                    await ExecuteSequential(eventData);
                    break;
                // 並行実行
                case ExecutionType.Parallel:
                    await ExecuteParallel(eventData);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ゲームイベントを順次実行していく
        /// </summary>
        private async UniTask ExecuteSequential(GameEventExecutionData eventData)
        {
            try
            {
                foreach (var data in eventData.EventDataArray)
                {
                    var eventType = data.EventType;
                
                    if (!_handlers.TryGetValue(eventType, out var handler))
                    {
                        LogUtility.Warning($"未登録のイベントタイプです: {eventType}", LogCategory.System);
                        continue;
                    }
                
                    // 各イベントを順次実行（前のイベントが完了してから次を実行）
                    await handler.HandleGameEvent(data.Parameters);
                }
            }
            catch (Exception ex)
            {
                LogUtility.Error($"オーダー実行中にエラーが発生: {ex.Message}", LogCategory.System);
            }
        }

        /// <summary>
        /// ゲームイベントを並列実行
        /// </summary>
        private async UniTask ExecuteParallel(GameEventExecutionData eventData)
        {
            try
            {
                var tasks = new List<UniTask>();

                foreach (var data in eventData.EventDataArray)
                {
                    var eventType = data.EventType;

                    if (!_handlers.TryGetValue(eventType, out var handler))
                    {
                        LogUtility.Warning($"未登録のイベントタイプです: {eventType}", LogCategory.System);
                        continue;
                    }

                    // 各イベントのタスクをリストに追加（実行はまだしない）
                    tasks.Add(handler.HandleGameEvent(data.Parameters));
                }

                // 全てのタスクを並列実行し、全て完了するまで待機
                await UniTask.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                LogUtility.Error($"オーダー実行中にエラーが発生: {ex.Message}", LogCategory.System);
            }
        }
    }
}