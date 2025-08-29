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
            base.AddClearData(storyId);
            
            // クリア時のコールバックを呼び出す
            OnStorySave?.Invoke(storyId);
        }
    }   
}