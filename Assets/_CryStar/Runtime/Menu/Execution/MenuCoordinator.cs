using CryStar.Menu.Enums;

namespace CryStar.Menu.Execution
{
    /// <summary>
    /// メニューシステムのMVP-Cパターンを管理するマネージャークラス
    /// </summary>
    public class MenuCoordinator : CoordinatorManagerBase
    {
        /// <summary>
        /// コーディネーターを切り替える
        /// </summary>
        public void TransitionToMenu(MenuSystemState menuType)
        {
            base.TransitionTo((int)menuType);
        }
    }
}
