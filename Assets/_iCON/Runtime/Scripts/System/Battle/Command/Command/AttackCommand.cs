using Cysharp.Threading.Tasks;
using UnityEngine;

namespace iCON.Battle
{
    /// <summary>
    /// 攻撃コマンド
    /// </summary>
    public class AttackCommand : IBattleCommand
    {
        // 優先順は特に設定なし
        public int Priority => 50;
        public string DisplayName => "攻撃";
        
        public bool CanExecute(BattleUnit executor)
        {
            // 使用者が生きていれば実行可能
            return executor.IsAlive;
        }
        
        public async UniTask<BattleCommandResult> ExecuteAsync(BattleUnit executor, BattleUnit[] targets)
        {
            if (targets.Length == 0)
            {
                // 敵がいない場合はコマンド失敗としてリザルトを作成
                return new BattleCommandResult(false, "対象が存在しません");
            }
            
            // 単体攻撃
            var target = targets[0];
            
            // ダメージ計算を行う
            int damage = CalculateDamage(executor, target);
            
            // クリティカル判定を行う
            bool isCritical = CheckCritical(executor.UserData.CriticalRate);
            if (isCritical)
            {
                // 小数点以下は丸める
                // NOTE: クリティカルダメージは100%といった単位で渡されるので、0.01fをかける
                damage = Mathf.RoundToInt(damage * executor.UserData.CriticalDamage * 0.01f);
            }
            
            // 演出を実行する
            await PlayAttackEffectAsync(target, isCritical);
            
            // ダメージ適用
            target.TakeDamage(damage);
            
            // エフェクトデータ作成
            var effects = new BattleEffectData[]
            {
                new BattleEffectData
                {
                    Target = target,
                    Damage = damage,
                    EffectName = "攻撃",
                    IsCritical = isCritical
                }
            };
            
            // ログに表示するメッセージを作成
            string message = isCritical ? 
                $"会心の一撃！{target.Name}に{damage}のダメージ！" :
                $"{target.Name}に{damage}のダメージ！";
            
            return new BattleCommandResult(true, message, effects);
        }
        
        /// <summary>
        /// ダメージ計算を行う
        /// </summary>
        private int CalculateDamage(BattleUnit attacker, BattleUnit defender)
        {
            // 基本ダメージ計算式
            // 攻撃力: アタッカー物理攻撃
            int baseDamage = attacker.Attack;
            // 実効防御力: ディフェンダーの物理防御 × (1 - アタッカーの防御無視率)
            int defense = (int)(defender.Defense * (1 - attacker.ArmorPenetration / 100f));
            // 最終物理ダメージ = 物理攻撃 × (100 / (100 + 実効防御力))
            int damage = Mathf.Max(1, (int)(baseDamage * (100f / (100f + defense))));
            
            // ランダム要素を追加（±10%）
            float randomFactor = Random.Range(0.9f, 1.1f);
            damage = Mathf.RoundToInt(damage * randomFactor);
            
            return damage;
        }
        
        /// <summary>
        /// クリティカル攻撃か抽選を行う
        /// </summary>
        private bool CheckCritical(int criticalLate)
        {
            return Random.Range(0, 100) < criticalLate;
        }
        
        /// <summary>
        /// 攻撃演出を実行する
        /// </summary>
        private async UniTask PlayAttackEffectAsync(BattleUnit target, bool isCritical)
        {
            // 現在は最小限の待機のみ
            // TODO: 将来的にエフェクト演出、アニメーション、サウンドなどを追加
            await UniTask.DelayFrame(1);
        }
    }
}