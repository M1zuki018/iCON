using System;

namespace CryStar.Data.User
{
    /// <summary>
    /// アイテムインベントリのユーザーデータ
    /// </summary>
    [Serializable]
    public class InventoryUserData : CachedUserDataBase
    {
        public InventoryUserData(int userId) : base(userId) { }
    }
}
