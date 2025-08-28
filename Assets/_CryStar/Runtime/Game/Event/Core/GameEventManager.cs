using System.Collections.Generic;
using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Game.Data;
using CryStar.Game.Enums;
using CryStar.Game.Events.Initialization;
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
        /// イベントIDとそのIDのイベントデータのkvp
        /// </summary>
        private Dictionary<int, GameEventData> _eventData = new Dictionary<int, GameEventData>();
        
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
        public async UniTask ExecuteEvent(int eventID)
        {
            var eventData = _eventData[eventID];
            var eventType = eventData.EventType;
            var handler = _handlers[eventType];
            
            await handler.HandleGameEvent(eventData.Parameter);
        }
    }
}
