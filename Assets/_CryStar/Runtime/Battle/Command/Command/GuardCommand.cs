using Cysharp.Threading.Tasks;

namespace iCON.Battle
{
    /// <summary>
    /// ガードコマンド
    /// </summary>
    public class GuardCommand : IBattleCommand
    {
        public int Priority => 3; // 優先度高め
        public string DisplayName => "ガード";
        
        public bool CanExecute(BattleUnit executor)
        {
            // 使用者が生存していれば実行可能
            return executor.IsAlive;
        }
        
        public async UniTask<BattleCommandResult> ExecuteAsync(BattleUnit executor, BattleUnit[] targets)
        {
            // Unitの状態をガード中に変更
            executor.IsGuarding = true;
            
            // 演出を実行する
            await PlayGuardEffectAsync();
            
            string message = $"{executor.Name}は身を守っている！";
            
            return new BattleCommandResult(true, message);
        }
        
        /// <summary>
        /// 攻撃演出を実行する
        /// </summary>
        private async UniTask PlayGuardEffectAsync()
        {
            // 現在は最小限の待機のみ
            // TODO: 将来的にエフェクト演出、アニメーション、サウンドなどを追加
            await UniTask.DelayFrame(1);
        }
    }
}