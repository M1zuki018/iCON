using UnityEngine;
using UnityEngine.UI;

namespace CryStar.Boot.UI
{
    /// <summary>
    /// 開始シーンを選択するためのドロップダウンコンポーネント
    /// </summary>
    [RequireComponent(typeof(Dropdown))]
    public class SceneSelectionDropdown : MonoBehaviour
    {
        private int _selectedSceneIndex = 0;
        private Dropdown _dropdown;
        
        /// <summary>
        /// 現在選択されているシーンのビルドインデックス
        /// </summary>
        public int SelectedSceneIndex => _selectedSceneIndex;

        private void Awake()
        {
            _dropdown = GetComponent<Dropdown>();
            
            // ドロップダウンの値変更アクションを登録
            _dropdown.onValueChanged.RemoveAllListeners();
            _dropdown.onValueChanged.AddListener(ChangeSelectedSceneIndex);
        }

        /// <summary>
        /// 開始するシーンが変更された通知を受け取り選択中のIndexを更新
        /// </summary>
        private void ChangeSelectedSceneIndex(int index)
        {
            _selectedSceneIndex = index;
        }
    }
}