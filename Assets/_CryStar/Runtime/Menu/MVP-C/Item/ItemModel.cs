using CryStar.Core;
using iCON.Enums;

namespace CryStar.Menu
{
    /// <summary>
    /// Item_Model
    /// </summary>
    public class ItemModel
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