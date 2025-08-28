using System;
using System.Collections.Generic;
using System.Linq;
using CryStar.Core;
using CryStar.Game.Events;
using CryStar.Utility;
using CryStar.Utility.Enum;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CryStar.Field.Event
{
    /// <summary>
    /// ストーリー再生を行うフィールドイベント
    /// </summary>
    public class FieldEvent_Story : FieldEventBase
    {
        /// <summary>
        /// 再生するイベントのID
        /// </summary>
        [SerializeField, Tooltip("カンマ区切りでストーリーIDを指定 (例: 1,2,3)")] 
        private string _playStoryId;
        
        /// <summary>
        /// パースされたストーリーIDのリスト
        /// </summary>
        private List<int> _idList = new List<int>();

        /// <summary>
        /// GameEventManager
        /// </summary>
        private GameEventManager _eventManager;

        #region Life cycle

        /// <summary>
        /// Start
        /// </summary>
        protected override void Start()
        {
            base.Start();
            InitializeStoryIdList();
        }

        #endregion
        
        /// <summary>
        /// Colliderにプレイヤーが触れた時の処理
        /// </summary>
        protected override void OnPlayerEnter(Collider2D playerCollider)
        {
            if (_eventManager == null)
            {
                // nullだったらInGameManagerを取得する
                _eventManager = ServiceLocator.GetGlobal<GameEventManager>();
            }
            
            // ストーリーID取得のためのindexを計算する
            // NOTE: 基本はオブジェクトに触れた回数。Cacheリストの範囲内になるように調節している
            var index = Mathf.Min(Count, _idList.Count);
            
            // 再生
            _eventManager.PlayEvent(_idList[index]).Forget();
        }
        
        /// <summary>
        /// ストーリーのIDリストの初期化を行う
        /// </summary>
        private void InitializeStoryIdList()
        {
            if (string.IsNullOrEmpty(_playStoryId))
            {
                // IDの指定がnullもしくは空であればログだけ出して早期return
                LogUtility.Warning("ストーリーIDが指定されていません", LogCategory.Gameplay, this);
                return;
            }
            
            try
            {
                _idList = _playStoryId
                    .Split(',')
                    .Select(id => id.Trim())
                    .Where(id => !string.IsNullOrEmpty(id))
                    .Select(int.Parse)
                    .ToList();
            }
            catch (Exception ex)
            {
                LogUtility.Error($"ストーリーIDのパースに失敗: {_playStoryId}\nエラー: {ex.Message}", LogCategory.Gameplay, this);
                _idList = null;
            }
        }
    }
}