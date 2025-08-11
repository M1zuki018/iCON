using System;
using CryStar.Utility;
using CryStar.Utility.Enum;
using UnityEngine;
using Random = UnityEngine.Random;

namespace iCON.Battle
{
    /// <summary>
    /// バトル時に使用するキャラクターデータ
    /// </summary>
    public class BattleUnit
    {
        public BattleUnitMaster Data { get; set; }
        public BattleUnitUserData UserData { get; set; }
        public int CurrentHp { get; set; }
        public int CurrentWill { get; set; }
        public int CurrentStamina { get; set; }
        public int CurrentSp { get; set; }
        public int PhysicalAttack { get; set; }
        public int SkillAttack { get; set; }
        public int Intelligence { get; set; }
        public int PhysicalDefense { get; set; }
        public int SkillDefense { get; set; }
        public int Speed { get; set; }
        public int DodgeSpeed { get; set; }
        public int ArmorPenetration { get; set; }
        
        /// <summary>
        /// ガード中
        /// </summary>
        public bool IsGuarding { get; set; }
        
        public string Name => Data.name;
        
        /// <summary>
        /// 生存しているか
        /// </summary>
        public bool IsAlive => CurrentHp > 0;
        
        /// <summary>
        /// HP変動を通知するコールバック
        /// </summary>
        public event Action<int, int, int> OnHpChanged;
        
        /// <summary>
        /// スキルポイント変動を通知するコールバック
        /// </summary>
        public event Action<int, int> OnSpChanged;

        /// <summary>
        /// 死亡通知
        /// </summary>
        public event Action OnDeath;

        /// <summary>
        /// 回避成功
        /// </summary>
        public event Action OnSuccessDodge;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">キャラクターID</param>
        public BattleUnit(int id, BattleUnitMaster data)
        {
            // TODO: idを元にデータを検索

            Data = data; // 仮
            UserData = new BattleUnitUserData();
            
            if (Data == null)
            {
                LogUtility.Error($"{id} のBattleUnitMasterがありません", LogCategory.Gameplay);
                return;
            }
            
            if (UserData == null)
            {
                LogUtility.Error($"{id} のBattleUnitUserDataがありません", LogCategory.Gameplay);
                return;
            }
            
            // TODO: ユーザーデータ参照
            CurrentHp = Data.Hp;
            CurrentWill = Data.Will;
            CurrentStamina = Data.Stamina;
            CurrentSp = Data.Sp;
            
            // マスタデータ参照
            PhysicalAttack = Data.PhysicalAttack;
            SkillAttack = Data.SkillAttack;
            Intelligence = Data.Intelligence;
            PhysicalDefense = Data.PhysicalDefense;
            SkillDefense = Data.SkillDefense;
            Speed = Data.Speed;
            DodgeSpeed = Data.DodgeSpeed;
            ArmorPenetration = Data.ArmorPenetration;
        }

        /// <summary>
        /// ダメージを受ける
        /// </summary>
        public void TakeDamage(int damage)
        {
            if (TryDodge())
            {
                // 回避成功したらイベント発火してリターン
                OnSuccessDodge?.Invoke();
                LogUtility.Info($"{Name} 回避成功！", LogCategory.Gameplay);
                return;
            }
            
            // 最小値は0、最大値はMaxHPにおさまるように調整
            var value = Mathf.Max(0, CurrentHp - damage);
            CurrentHp = Mathf.Min(value, Data.Hp);
            OnHpChanged?.Invoke(CurrentHp, Data.Hp, damage);

            if (CurrentHp <= 0)
            {
                // 現在HPが0以下になったときには死亡イベントを呼び出す
                OnDeath?.Invoke();
            }
        }

        /// <summary>
        /// SPを消費する
        /// </summary>
        public void ConsumedSp(int amount)
        {
            // 最小値は0、最大値はMaxSPにおさまるように調整
            var value = Mathf.Max(0, CurrentSp - amount);
            CurrentHp = Mathf.Min(value, Data.Sp);
            OnSpChanged?.Invoke(CurrentSp, Data.Sp);
        }

        /// <summary>
        /// 回避チェック
        /// </summary>
        private bool TryDodge()
        {
            // 乱数生成
            float randomFactor = Random.Range(0f, 100f);
            // 生成した乱数が回避速度以下なら成功
            return randomFactor <= (DodgeSpeed + 100) * 0.01f;
        }

        /// <summary>
        /// 状態異常チェック
        /// </summary>
        private bool CheckAbnormalCondition()
        {
            float randomFactor = Random.Range(0f, 100f);
            return randomFactor <= (CurrentWill + 100) * 0.01f;
        }
    }
}
