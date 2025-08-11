using System;
using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Utility;
using UnityEngine;

namespace CryStar.Data
{
    /// <summary>
    /// ユーザーデータ管理クラス
    /// </summary>
    [DefaultExecutionOrder(-998)]
    public class UserDataManager : MonoBehaviour
    {
        private static int _userId = -1;
        private UserDataContainer _currentUserData;
        
        public UserDataContainer CurrentUserData => _currentUserData;

        private void Awake()
        {
            // グローバルサービスに登録しておく
            ServiceLocator.Register(this, ServiceType.Global);

            // TODO: 仮
            if (_userId < 0)
            {
                // UserIdが0以下 = 未設定の場合、GUIDを元にユーザーIDを生成する
                var guid = Guid.NewGuid();
                _userId = Math.Abs(guid.GetHashCode());
                LogUtility.Info($"UserDataを作成しました ID:{_userId}");
            }

            if (_currentUserData == null)
            {
                // UserDataが存在しない場合、データを作成する
                CreateUserData(_userId);
            }
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
