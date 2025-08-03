using System.Collections.Generic;
using CryStar.Data;
using CryStar.Field.Event;
using UnityEngine;

namespace CryStar.Field.Data
{
     public class FieldSaveData : BaseUserData
     {
          #region Private Field

          private int _currentMapId = 1; // 初期マップ
          private Vector2 _lastPosition = Vector2.zero;
          private Vector2 _lastRotation = Vector2.zero;
          private Dictionary<FieldEventBase, int> _clearedEventMap = new Dictionary<FieldEventBase, int>();

          #endregion

          /// <summary>
          /// 現在のマップID
          /// </summary>
          public int CurrentMapId => _currentMapId;

          /// <summary>
          /// 最後位置のPosition
          /// </summary>
          public Vector2 LastPosition => _lastPosition;

          /// <summary>
          /// 最終位置のRotation
          /// </summary>
          public Vector2 LastRotation => _lastRotation;

          /// <summary>
          /// クリア済みのイベントと回数のマッピング
          /// </summary>
          public Dictionary<FieldEventBase, int> ClearedEventMap => _clearedEventMap;

          /// <summary>
          /// コンストラクタ
          /// </summary>
          public FieldSaveData(int userId) : base(userId)
          {
               _currentMapId = 1; // 初期マップ
               _lastPosition = Vector3.zero;
               _lastRotation = Vector3.zero;
               _clearedEventMap = new Dictionary<FieldEventBase, int>();
          }

          /// <summary>
          /// マップIDを更新
          /// </summary>
          public void TransitionMap(int newMapId)
          {
               _currentMapId = newMapId;
          }

          /// <summary>
          /// 最終位置と回転を更新する
          /// </summary>
          public void SetLastTranslation(Vector2 position, Vector2 rotation)
          {
               _lastPosition = position;
               _lastRotation = rotation;
          }

          /// <summary>
          /// イベントをクリアした際に呼び出す
          /// </summary>
          public void ClearEvent(FieldEventBase fieldEvent)
          {
               if (_clearedEventMap.ContainsKey(fieldEvent))
               {
                    // 既にクリア済みで辞書に登録されていれば、クリア回数を増やす
                    _clearedEventMap[fieldEvent]++;
               }

               _clearedEventMap[fieldEvent] = 1;
          }

          /// <summary>
          /// イベントがクリア済みかチェック
          /// </summary>
          public bool IsEventCleared(FieldEventBase fieldEvent)
          {
               // クリアしたときに辞書に登録されるため、辞書にキーが存在するかを調べる
               return _clearedEventMap.ContainsKey(fieldEvent);
          }
     }
}