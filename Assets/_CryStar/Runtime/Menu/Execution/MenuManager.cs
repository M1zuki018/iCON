using CryStar.Attribute;
using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Menu.Enums;
using iCON.Enums;
using iCON.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryStar.Menu.Execution
{
    /// <summary>
    /// メニューシステムを管理するクラス
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        /// <summary>
        /// InGameのUI管理クラス
        /// </summary>
        [SerializeField, HighlightIfNull]
        private InGameCanvasManager _view;
        
        /// <summary>
        /// Coordinator Manager
        /// </summary>
        [SerializeField, HighlightIfNull]
        private MenuCoordinator _menuCoordinator;
        
        /// <summary>
        /// メニューの状態
        /// </summary>
        [FormerlySerializedAs("_currentState")] [SerializeField]
        private MenuStateType currentStateType;
        
        /// <summary>
        /// InGameのUI管理クラス
        /// </summary>
        public InGameCanvasManager View => _view;
        
        /// <summary>
        /// Coordinator Manager
        /// </summary>
        public MenuCoordinator MenuCoordinator => _menuCoordinator;

        #region Life cycle

        private void Start()
        {
            ServiceLocator.Register(this, ServiceType.Local);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (currentStateType == MenuStateType.None)
                {
                    // 現在メニューを開いていない状態だったらメニューを開いて早期リターン
                    _view.GetCanvas(InGameCanvasType.MainMenu).Show();
                    _menuCoordinator.TransitionToMenu(MenuStateType.MainMenu);
                    return;
                }
                
                // Escapeキーが押されたら各ハンドルの画面を閉じる処理を呼び出す
                _menuCoordinator.CurrentCoordinator?.Cancel();
            }
        }

        #endregion
    }
}

