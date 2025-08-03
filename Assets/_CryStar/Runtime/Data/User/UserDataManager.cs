using CryStar.Core;
using CryStar.Core.Enums;
using Cysharp.Threading.Tasks;

namespace CryStar.Data
{
    /// <summary>
    /// ユーザーデータ管理クラス
    /// </summary>
    public class UserDataManager : CustomBehaviour
    {
        private UserDataContainer _currentUserData;
        public UserDataContainer CurrentUserData => _currentUserData;

        public override async UniTask OnAwake()
        {
            // グローバルサービスに登録しておく
            await base.OnAwake();
            ServiceLocator.Register(this, ServiceType.Global);
        }

        /// <summary>
        /// ユーザーデータを作成する
        /// </summary>
        public void CreateUserData(int userId)
        {
            _currentUserData = new UserDataContainer(userId);
        }

        /// <summary>
        /// ユーザーデータを保存
        /// </summary>
        public void SaveUserData()
        {
            if (_currentUserData == null) return;

            _currentUserData.UpdateAllSaveTimes();
        }
    }
}
