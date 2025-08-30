using CryStar.Core;
using iCON.Enums;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// Lose_Model
    /// </summary>
    public class LoseModel
    {
        /// <summary>
        /// BattleManager
        /// </summary>
        private BattleManager _battleManager;

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            _battleManager = ServiceLocator.GetLocal<BattleManager>();
        }

        /// <summary>
        /// 処理を実行する
        /// </summary>
        public void Enter()
        {
            // BGM再生を止める
            _battleManager.FinishBGM();
            _battleManager.View.ShowCanvas(BattleCanvasType.Lose);
        }

        /// <summary>
        /// バトルマネージャーが取得できているか確認し、取得できていなかったらServiceLocatorから取得する
        /// </summary>
        private void TryGetBattleManager()
        {
            if (_battleManager == null)
            {
                _battleManager = ServiceLocator.GetLocal<BattleManager>();
            }
        }
    }
}