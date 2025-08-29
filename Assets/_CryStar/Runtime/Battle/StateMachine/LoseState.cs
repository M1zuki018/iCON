using iCON.Enums;
using iCON.UI;

namespace iCON.Battle
{
    /// <summary>
    /// バトル敗北
    /// </summary>
    public class LoseState : BattleStateBase
    {
        public override void Enter(BattleManager manager, BattleCanvasManager view)
        {
            base.Enter(manager, view);
            
            // BGM再生を止める
            manager.FinishBGM();
            
            view.ShowCanvas(BattleCanvasType.Lose);
        }
    }
}