using System;
using System.Collections.Generic;

namespace iCON
{
    /// <summary>
    /// キャラクターのセーブデータ
    /// </summary>
    [Serializable]
    public class CharacterData
    {
        private int _characterID;
        
        /// <summary>
        /// キャラクターのID
        /// </summary>
        public int CharacterID => _characterID;

        /// <summary>
        /// レベル
        /// </summary>
        public int Level { get; set; } = 1;
        
        /// <summary>
        /// 経験値
        /// </summary>
        public int Experience { get; set; } = 0;
        
        // フィールドでの現在値（持ち越し用）
        // NOTE: -1は未設定
        
        /// <summary>
        /// 現在のHPの減少量
        /// </summary>
        public int DecreaseHp { get; set; } = 0;
        
        /// <summary>
        /// 現在のSPの減少量
        /// </summary>
        public int DecreaseSp { get; set; } = 0;
        
        // 装備やアイテムによる補正値
        
        /// <summary>
        /// HP補正値
        /// </summary>
        public int BonusHp { get; set; } = 0;
        
        /// <summary>
        /// SP補正値
        /// </summary>
        public int BonusSp { get; set; } = 0;
        
        /// <summary>
        /// 攻撃力補正値
        /// </summary>
        public int BonusAttack { get; set; } = 0;
        
        /// <summary>
        /// 防御力補正値
        /// </summary>
        public int BonusDefense { get; set; } = 0;

        /// <summary>
        /// スキル倍率（float）
        /// </summary>
        public float BonusSkillMultiplier { get; set; } = 0f;
        
        /// <summary>
        /// 状態異常耐性
        /// </summary>
        public int BonusStatusResistance { get; set; } = 0;

        /// <summary>
        /// 攻撃速度
        /// </summary>
        public int BonusSpeed { get; set; } = 0;
        
        /// <summary>
        /// 回避速度
        /// </summary>
        public int BonusDodgeSpeed { get; set; } = 0;
        
        /// <summary>
        /// 防御無視
        /// </summary>
        public int BonusArmorPenetration { get; set; } = 0;

        /// <summary>
        /// クリティカル率
        /// </summary>
        public int BonusCriticalRate { get; set; } = 0;
        
        /// <summary>
        /// クリティカルダメージ
        /// </summary>
        public int BonusCriticalDamage { get; set; } = 0;
        
        /// <summary>
        /// IdeaのIDリスト
        /// </summary>
        public List<int> IdeaIdList { get; set; } = new List<int>();
        
        // TODO: 装備データの変数も作成する

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CharacterData(int characterID)
        {
            _characterID = characterID;
        }
    }
}