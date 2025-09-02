using CryStar.Menu.UI;
using UnityEngine;

namespace CryStar.Menu
{
    /// <summary>
    /// CharacterStatus_View
    /// </summary>
    public class CharacterStatusView : MonoBehaviour
    {
        /// <summary>
        /// ステータス表記のコンテンツ
        /// </summary>
        [SerializeField] 
        private UIContents_Status _status;
        
        /// <summary>
        /// 初期状態で表示されるキャラクターのID
        /// </summary>
        [SerializeField] 
        private int _defaultCharacterId = 1;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            // TODO: キャラクターを切り替えるボタンを追加→ボタンが押されたときに表示されるキャラクターを切り替える処理
            _status.Setup(_defaultCharacterId);
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            // Actionの解放処理
        }
    }
}