using System;

namespace CryStar.Data.User
{
    /// <summary>
    /// ストーリーのユーザーデータ
    /// </summary>
    [Serializable]
    public class StoryUserData : CachedUserDataBase
    {
        /// <summary>
        /// ストーリーがセーブされたときのコールバック
        /// </summary>
        public event Action<int> OnStorySave;
        
        public StoryUserData(int userId) : base(userId) { }
        
        /// <summary>
        /// クリアしたか
        /// </summary>
        public override void AddClearData(int storyId)
        {
            if (!_clearedDataCache.ContainsKey(storyId))
            {
                // 未クリアの場合、コールバックを呼び出す
                OnStorySave?.Invoke(storyId);
            }
            
            base.AddClearData(storyId);
        }
    }   
}