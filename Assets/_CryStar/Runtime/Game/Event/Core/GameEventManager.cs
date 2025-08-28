using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Game.Events.Initialization;
using Cysharp.Threading.Tasks;

namespace CryStar.Game.Events
{
    /// <summary>
    /// Game Event Manager
    /// </summary>
    public class GameEventManager : CustomBehaviour
    {
        /// <summary>
        /// Awake
        /// </summary>
        public override async UniTask OnAwake()
        {
            await base.OnAwake();
            
            // インゲーム・バトルシーンで利用するためGlobalサービスに登録
            ServiceLocator.Register(this, ServiceType.Global);
            
            // 念のためゲームイベントシステムが初期化されていることを確認する
            GameEventInitializer.Initialize();
        }
    }
}
