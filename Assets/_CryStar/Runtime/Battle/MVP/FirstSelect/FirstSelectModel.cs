using CryStar.Core;
using Cysharp.Threading.Tasks;
using iCON.Battle;
using iCON.Enums;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// FirstSelect_Model
    /// </summary>
    public class FirstSelectModel
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
        /// バトルを開始してコマンド選択に移る
        /// </summary>
        public void StartBattle()
        {
            TryGetBattleManager();   
            _battleManager.PlaySelectedSe(true).Forget();
            _battleManager.View.ShowCanvas(BattleCanvasType.CommandSelect);
        }

        /// <summary>
        /// 逃走チェック
        /// </summary>
        public void TryEscape()
        {
            TryGetBattleManager();
            _battleManager.PlaySelectedSe(false).Forget();
            _battleManager.View.ShowCanvas(BattleCanvasType.TryEscape);
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