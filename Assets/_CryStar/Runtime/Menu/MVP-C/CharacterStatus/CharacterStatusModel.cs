using CryStar.Core;
using CryStar.Menu.Enums;
using CryStar.Menu.Execution;
using iCON.Enums;

namespace CryStar.Menu
{
    /// <summary>
    /// CharacterStatus_Model
    /// </summary>
    public class CharacterStatusModel
    {
        /// <summary>
        /// MenuManager
        /// </summary>
        private MenuManager _manager;

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            // TODO
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel()
        {
            TryGetMenuManager();
            // メインメニューへ遷移
            _manager.MenuCoordinator.TransitionToMenu(MenuStateType.MainMenu);
        }
        
        /// <summary>
        /// メニューマネージャーが取得できているか確認し、取得できていなかったらServiceLocatorから取得する
        /// </summary>
        private void TryGetMenuManager()
        {
            if (_manager == null)
            {
                _manager = ServiceLocator.GetLocal<MenuManager>();
            }
        }
    }
}