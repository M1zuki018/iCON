using System;
using System.Collections.Generic;
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
        
        /// <summary>
        /// 現在使用中のユーザーデータ
        /// </summary>
        private UserDataContainer _currentUserData;
        
        /// <summary>
        /// 保存されている全てのユーザーデータ
        /// </summary>
        private List<UserDataContainer> _userDataContainers = new List<UserDataContainer>();
        
        /// <summary>
        /// 現在使用中のユーザーデータ
        /// </summary>
        public UserDataContainer CurrentUserData => _currentUserData;
        
        /// <summary>
        /// 保存されているユーザーデータの数を取得
        /// </summary>
        public int UserDataCount => _userDataContainers.Count;

        private void Awake()
        {
            // グローバルサービスに登録しておく
            ServiceLocator.Register(this, ServiceType.Global);

            if (_userId < 0)
            {
                // UserIdが0以下 = 未設定の場合、GUIDを元にユーザーIDを生成する
                var guid = Guid.NewGuid();
                _userId = Math.Abs(guid.GetHashCode());
                LogUtility.Info($"UserIDを生成しました ID:{_userId}");
            }
            
            if (_userDataContainers.Count > 0)
            {
                // セーブデータが存在する場合は最後に使用していたデータを選択（または最新のデータ）
                TrySelectUserData(0);
            }
        }

        /// <summary>
        /// ユーザーデータを作成する
        /// </summary>
        public void CreateUserData()
        {
            // 新規ユーザーデータを作成しリストに保存する
            _userDataContainers.Add(new UserDataContainer(_userId));
            
            // 最新のユーザーデータを現在利用中のユーザーデータ変数にキャッシュ
            _currentUserData = _userDataContainers[_userDataContainers.Count - 1];
            
            LogUtility.Info($"セーブデータを作成しました。Index: {_userDataContainers.Count - 1}, Count: {_userDataContainers.Count}");
        }

        /// <summary>
        /// 使用するユーザーデータを選択する
        /// </summary>
        public bool TrySelectUserData(int index)
        {
            if (index < 0 || index >= _userDataContainers.Count)
            {
                LogUtility.Warning($"無効なインデックスです。Index: {index}, Count: {_userDataContainers.Count}");
                return false;
            }
            
            var userData = _userDataContainers[index];

            if (userData == null)
            {
                LogUtility.Warning($"セーブデータが存在しません。Index: {index}, Count: {_userDataContainers.Count}");
                return false;
            }
            
            _currentUserData = userData;
            LogUtility.Info($"セーブデータを選択しています。Index: {index}, Count: {_userDataContainers.Count}");
            return true;
        }

        /// <summary>
        /// ユーザーデータを保存
        /// </summary>
        public void SaveUserData()
        {
            if (_currentUserData == null) return;

            _currentUserData.UpdateAllSaveTimes();
            LogUtility.Info("ユーザーデータを保存しました");
        }
        
        /// <summary>
        /// ユーザーデータを削除
        /// </summary>
        public void DeleteUserData(int index)
        {
            if (!IsValidIndex(index))
            {
                LogUtility.Error($"削除対象のインデックスが無効です。Index: {index}");
                return;
            }
    
            _userDataContainers.RemoveAt(index);
    
            // 現在選択中のデータが削除された場合の処理
            if (_currentUserData == _userDataContainers[index])
            {
                _currentUserData = _userDataContainers.Count > 0 ? _userDataContainers[0] : null;
            }
        }
        
        /// <summary>
        /// 指定インデックスのユーザーデータが存在するかチェック
        /// </summary>
        public bool IsValidIndex(int index)
        {
            return index >= 0 && index < _userDataContainers.Count;
        }
    }
}
