using CryStar.CommandBattle.Enums;

namespace CryStar.CommandBattle.Execution
{
    /// <summary>
    /// バトルシーンのMVP-Cパターンを管理するマネージャークラス
    /// </summary>
    public class BattleCoordinator : CoordinatorManagerBase
    {
        /// <summary>
        /// キャンバスを切り替える
        /// </summary>
        public void ShowCanvas(BattlePhaseType phaseType)
        {
            base.ShowCanvas((int)phaseType);
        }

        /// <summary>
        /// キャンバスを開き直す
        /// NOTE: 通常のShowCanvasメソッドだと同じCanvasを開こうとしたときにreturnされてしまうので
        /// こちらのメソッドを使う
        /// </summary>
        public void ShowCanvasReopen(BattlePhaseType phaseType)
        {
            base.ShowCanvasReopen((int)phaseType);
        }
    }
}
