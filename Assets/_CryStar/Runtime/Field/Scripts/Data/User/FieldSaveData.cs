using System;
using System.Collections.Generic;
using CryStar.Data;
using CryStar.Field.Event;
using CryStar.Utility;
using CryStar.Utility.Enum;
using iCON.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryStar.Field.Data
{
     [Serializable]
     public class FieldSaveData : BaseUserData
     {
          #region Private Field

          [SerializeField] private int _lastMapId = 1; // 初期マップ
          [SerializeField] private Vector2 _lastPosition = Vector2.zero;
          [SerializeField] private MoveDirectionType _directionType = MoveDirectionType.Down;
          [SerializeField] private List<EventClearData> _clearedEvents;
          
          private Dictionary<int, int> _eventClearCache; // 実行時のパフォーマンス向上のためのキャッシュ
          
          #endregion

          /// <summary>
          /// 最終位置のマップID
          /// </summary>
          public int LastMapId => _lastMapId;

          /// <summary>
          /// 最後位置のPosition
          /// </summary>
          public Vector2 LastPosition => _lastPosition;

          /// <summary>
          /// 最終で向いている方向
          /// </summary>
          public MoveDirectionType DirectionType => _directionType;

          /// <summary>
          /// クリア済みのイベントと回数のマッピング
          /// </summary>
          public List<EventClearData> ClearedEvents => _clearedEvents;

          /// <summary>
          /// コンストラクタ
          /// </summary>
          public FieldSaveData(int userId) : base(userId)
          {
               _lastMapId = 1; // 初期マップ
               _lastPosition = Vector3.zero;
               _directionType = MoveDirectionType.Down;
               _clearedEvents = new List<EventClearData>();
               
               // 実行時用のディクショナリーを構築する
               BuildCache();
          }

          /// <summary>
          /// マップIDを更新
          /// </summary>
          public void TransitionMap(int newMapId)
          {
               // 1以下にならないようにする
               _lastMapId = Math.Max(newMapId, 1);
          }

          /// <summary>
          /// 最終位置と回転を更新する
          /// </summary>
          public void SetLastTranslation(Vector2 position, MoveDirectionType direction)
          {
               _lastPosition = position;
               _directionType = direction;
          }
          
          /// <summary>
          /// イベントをクリアした際に呼び出す
          /// </summary>
          public void ClearEvent(FieldEventBase fieldEvent)
          {
               if (fieldEvent == null)
               {
                    LogUtility.Warning("フィールドイベントがnullです", LogCategory.System);
                    return;
               }
               
               ClearEvent(fieldEvent.EventID);
          }
          
          /// <summary>
          /// イベントIDでクリアを記録
          /// </summary>
          public void ClearEvent(int eventId)
          {
               // キャッシュを更新
               if (_eventClearCache.ContainsKey(eventId))
               {
                    _eventClearCache[eventId]++;
               }
               else
               {
                    _eventClearCache[eventId] = 1;
               }
            
               // シリアライズ用リストを更新
               var existingData = _clearedEvents.Find(x => x.EventId == eventId);
               if (existingData != null)
               {
                    existingData.ClearCount = _eventClearCache[eventId];
               }
               else
               {
                    _clearedEvents.Add(new EventClearData(eventId, 1));
               }
          }

          /// <summary>
          /// イベントがクリア済みかチェック
          /// </summary>
          public bool IsEventCleared(FieldEventBase fieldEvent)
          {
               // クリアしたときに辞書に登録されるため、辞書にキーが存在するかを調べる
               return _eventClearCache.ContainsKey(fieldEvent.EventID);
          }
          
          /// <summary>
          /// パフォーマンス向上のためのキャッシュを構築
          /// </summary>
          private void BuildCache()
          {
               _eventClearCache = new Dictionary<int, int>();
            
               if (_clearedEvents != null)
               {
                    foreach (var eventData in _clearedEvents)
                    {
                         _eventClearCache[eventData.EventId] = eventData.ClearCount;
                    }
               }
          }
     }
}