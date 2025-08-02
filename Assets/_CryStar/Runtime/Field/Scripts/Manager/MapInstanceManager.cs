using System.Collections.Generic;
using CryStar.Core;
using UnityEngine;

namespace CryStar.Field.Manager
{
    /// <summary>
    /// フィールドマップの生成・非表示などを行う
    /// </summary>
    public class MapInstanceManager : CustomBehaviour
    {
        /// <summary>
        /// 既に生成済みのマップの辞書
        /// </summary>
        private Dictionary<int, GameObject> _instantiatedMaps = new Dictionary<int, GameObject>();

        #region Life cycle

        /// <summary>
        /// Destroy
        /// </summary>
        private void OnDestroy()
        {
            DestoryAllMap();
        }

        #endregion
        
        /// <summary>
        /// 指定したマップを表示状態にする
        /// </summary>
        public void ShowMap(int mapId)
        {
            if (_instantiatedMaps.ContainsKey(mapId))
            {
                // 既に生成済みであればアクティブ状態にする
                _instantiatedMaps[mapId].SetActive(true);
                return;
            }
            
            // 生成済み出ない場合は生成処理を行う
            CreateMapInstance(mapId);
        }

        /// <summary>
        /// 指定したマップを非表示にする
        /// </summary>
        public void DisableMap(int mapId)
        {
            if (_instantiatedMaps.ContainsKey(mapId))
            {
                _instantiatedMaps[mapId].SetActive(false);
            }
        }

        /// <summary>
        /// 指定したマップインスタンスを削除する
        /// </summary>
        public void RemoveMap(int mapId)
        {
            if (_instantiatedMaps.TryGetValue(mapId, out var mapObject))
            {
                if (mapObject != null)
                {
                    // オブジェクトの削除も行う
                    Destroy(mapObject);
                }
                _instantiatedMaps.Remove(mapId);
            }
        }

        /// <summary>
        /// 全てのマップインスタンスを削除する
        /// </summary>
        public void DestoryAllMap()
        {
            foreach (var kvp in _instantiatedMaps)
            {
                if (kvp.Value != null)
                {
                    // オブジェクトの削除
                    Destroy(kvp.Value);
                }
            }
            
            // 辞書をクリア
            _instantiatedMaps.Clear();
        }
        
        /// <summary>
        /// 指定したマップが生成済みかどうかを判定する
        /// </summary>
        public bool IsMapInstantiated(int mapId)
        {
            return _instantiatedMaps.ContainsKey(mapId);
        }

        /// <summary>
        /// 指定したマップが表示状態かどうかを判定する
        /// </summary>
        public bool IsMapVisible(int mapId)
        {
            return _instantiatedMaps.TryGetValue(mapId, out var mapInstance) 
                   && mapInstance != null && mapInstance.activeInHierarchy;
        }

        #region Private Methods

        /// <summary>
        /// シーン内にオブジェクトを生成する
        /// </summary>
        private void CreateMapInstance(int mapId)
        {
            // プレハブを取得
            var mapPrefab = MasterMapData.GetMapPrefab(mapId);
            if (mapPrefab == null)
            {
                Debug.LogError($"マップのプレハブが見つかりませんでした mapId: {mapId}");
                return;
            }

            // 自身の子オブジェクトに追加
            var mapInstance = Instantiate(mapPrefab, transform);
            
            // 生成済みのオブジェクトの辞書に追加
            _instantiatedMaps[mapId] = mapInstance;
        }

        #endregion
    }
}
