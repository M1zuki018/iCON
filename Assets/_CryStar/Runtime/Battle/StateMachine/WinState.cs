using CryStar.Core;
using CryStar.Data.Scene;
using Cysharp.Threading.Tasks;
using iCON.Enums;
using iCON.System;
using iCON.UI;

namespace iCON.Battle
{
    /// <summary>
    /// バトル勝利
    /// </summary>
    public class WinState : BattleStateBase
    {
        public override async void Enter(BattleManager manager, BattleCanvasManager view)
        {
            base.Enter(manager, view);
            
            // BGM再生を止める
            manager.FinishBGM();
            
            view.ShowCanvas(BattleCanvasType.Win);

            var cc = view.CurrentCanvas as CanvasController_Win;
            if (cc != null)
            {
                var resultData = manager.GetResultData();
                
                // 戦闘結果のパネルを表示
                cc.SetText(resultData.name, resultData.experience);
                await UniTask.Delay(4000);   
            }
            
            await ServiceLocator.GetGlobal<SceneLoader>().LoadSceneAsync(new SceneTransitionData(SceneType.InGame, false, true));
        }
    }
}
