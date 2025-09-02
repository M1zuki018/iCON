using System.Collections.Generic;
using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Core.UserData;
using UnityEngine;

namespace CryStar.Menu.UI
{
    /// <summary>
    /// キャラクターステータス画面の大きな枠の単位のコンテンツ
    /// </summary>
    public class UIContents_Status : MonoBehaviour
    {
        /// <summary>
        /// ステータスコンテンツのリスト
        /// </summary>
        [SerializeField]
        private List<UIContents_Parameter> _parameters = new List<UIContents_Parameter>();

        private int _characterId = 0;

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(int characterId)
        {
            _characterId = characterId;
            Initialize();
        }
        
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            var userData = ServiceLocator.GetGlobal<UserDataManager>().CurrentUserData.CharacterUserData.GetCharacterUserData(_characterId);
            var level = userData.Level;
            
            SetValue(StatusType.Level, level);
            SetValue(StatusType.Hp, MasterCharacter.GetHp(_characterId, level) - userData.DecreaseHp + userData.BonusHp);
            SetValue(StatusType.Will, 5); // TODO
            SetValue(StatusType.Stamina,100); // TODO
            SetValue(StatusType.Sp,MasterCharacter.GetSp(_characterId, level) - userData.DecreaseSp + userData.BonusSp);
            SetValue(StatusType.PhysicalAttack, MasterCharacter.GetAttack(_characterId, level) + userData.BonusAttack);
            SetValue(StatusType.SkillAttack, MasterCharacter.GetAttack(_characterId, level) + userData.BonusAttack); // TODO
            SetValue(StatusType.Intelligence,MasterCharacter.GetStatusResistance(_characterId, level) + userData.BonusStatusResistance);
            SetValue(StatusType.PhysicalDefense, MasterCharacter.GetDefense(_characterId, level) + userData.BonusDefense);
            SetValue(StatusType.SkillDefense, MasterCharacter.GetDefense(_characterId, level) + userData.BonusDefense); // TODO
            SetValue(StatusType.Speed, MasterCharacter.GetSpeed(_characterId, level) + userData.BonusSpeed);
            SetValue(StatusType.DodgeSpeed, MasterCharacter.GetDodgeSpeed(_characterId, level) + userData.BonusDodgeSpeed);
            SetValue(StatusType.ArmorPenetration, MasterCharacter.GetArmorPenetration(_characterId, level) + userData.BonusArmorPenetration);
            SetValue(StatusType.CriticalRate, MasterCharacter.GetCriticalRate(_characterId, level) + userData.BonusCriticalRate);
            SetValue(StatusType.CriticalDamage, MasterCharacter.GetCriticalDamage(_characterId, level) + userData.BonusCriticalDamage);
        }
        
        /// <summary>
        /// 引数で指定したインデックスのステータスの値を更新する
        /// </summary>
        private void SetValue(StatusType statusType, int value)
        {
            _parameters[(int)statusType].SetValue(value);
        }
    }
}
