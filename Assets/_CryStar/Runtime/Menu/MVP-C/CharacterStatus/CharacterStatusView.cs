using CryStar.Core.Enums;
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
        [SerializeField] private UIContents_Status _status;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            _status.Setup(1);
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