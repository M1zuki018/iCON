using Cysharp.Threading.Tasks;
using iCON.Enums;
using iCON.UI;

namespace iCON.Battle
{
    /// <summary>
    /// バトル実行
    /// </summary>
    public class ExecuteState : BattleStateBase
    {
        private CanvasController_Execute _cc;
        
        public override async void Enter(BattleManager manager, BattleCanvasManager view)
        {
            base.Enter(manager, view);
            view.ShowCanvas(BattleCanvasType.Execute);

            if (_cc == null)
            {
                // nullの場合参照を取得する
                _cc = view.CurrentCanvas as CanvasController_Execute;
            }
            
            // コマンドの実行リストを整える
            var commandList = manager.CreateCommandList();

            // 全コマンドを実行する
            foreach (var entry in commandList)
            {
                if (!entry.Executor.IsAlive)
                {
                    // 実行者が死亡している場合はスキップ
                    continue;
                }
                
                // ログを表示
                _cc.SetText($"{entry.Executor.Name}の{entry.Command.DisplayName}");
                
                // 少し待つ
                await UniTask.Delay(1000);
                
                // 演出を実行し、実行メッセージを取得する
                var message = await manager.ExecuteCommandAsync(entry);
                
                // メッセージを表示
                _cc.SetText(message);
                
                // 少し待つ
                await UniTask.Delay(1000);
            }
            
            // バトル実行を待つ
            var result = await manager.CheckBattleEndAsync();

            if (result.isFinish)
            {
                // バトルが終了している場合は勝敗に合わせてステートを変更
                manager.SetState(result.isWin ? BattleSystemState.Win : BattleSystemState.Lose);
            }
            else
            {
                // バトルが終了していなければ最初の選択に戻る
                manager.SetState(BattleSystemState.FirstSelect);
            }
        }
    }

}