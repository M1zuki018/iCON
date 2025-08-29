using System;
using System.Collections.Generic;
using CryStar.Utility;
using CryStar.Utility.Enum;
using iCON.Battle.Data;

namespace CryStar.Data.User
{
    /// <summary>
    /// キャラクターステータスのユーザーデータ
    /// </summary>
    [Serializable]
    public class CharacterUserData : UserDataBase
    {
        /// <summary>
        /// キャラクターIDとユーザーデータのkvp
        /// </summary>
        private Dictionary<int, BattleCharacterData> _characters = new Dictionary<int, BattleCharacterData>();
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CharacterUserData(int userId) : base(userId)
        {
            if (_characters == null)
            {
                // nullの場合、辞書を生成する
                _characters = new Dictionary<int, BattleCharacterData>();
            }
            
            // Characterのマスターデータに登録されているキャラクター数の数だけ、ユーザーデータ生成処理を行う
            for (int i = 1; i <= MasterCharacter.RegisteredCharacterCount; i++)
            {
                var index = i;
                _characters[index] = new BattleCharacterData(index);
            }
        }
    
        /// <summary>
        /// 引数で指定したキャラクターのユーザーデータを取得する
        /// </summary>
        public BattleCharacterData GetCharacterUserData(int characterId)
        {
            if (_characters == null)
            {
                LogUtility.Fatal($"{typeof(CharacterUserData)} が初期化されていません", LogCategory.Gameplay);
                return null;
            }
            
            if (!_characters.TryGetValue(characterId, out var data))
            {
                // nullチェック
                LogUtility.Error($"{characterId} のUserDataが取得できませんでした", LogCategory.Gameplay);
                return null;
            }
            
            return data;
        }
    
        /// <summary>
        /// すべてのキャラクターデータを取得する
        /// </summary>
        public List<BattleCharacterData> GetAllCharacterUserData()
        {
            return new List<BattleCharacterData>(_characters.Values);
        }
    
        /// <summary>
        /// セーブデータ復元
        /// </summary>
        public void SetCharacterUserData(List<BattleCharacterData> characters)
        {
            foreach (var data in characters)
            {
                if (_characters.ContainsKey(data.CharacterID))
                {
                    _characters[data.CharacterID] = data;
                }
            }
        }
    }
}