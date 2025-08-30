using CryStar.Core;
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
            // TODO{ClassName}
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel()
        {
            TryGetMenuManager();
            // メインメニューへ遷移
            _manager.View.ShowCanvas(InGameCanvasType.MainMenu);
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