using CryStar.Attribute;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// CanvasController_Win
    /// </summary>
    public class CanvasController_Win : WindowBase
    {
        /// <summary>
        /// View
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private WinView _view;
        
        /// <summary>
        /// Presenter
        /// </summary>
        private WinPresenter _presenter = new WinPresenter();
        
        public override void Enter()
        {
            base.Enter();
            _presenter?.Setup(_view);
        }

        public override void Exit()
        {
            _presenter?.Exit();
            base.Exit();
        }
    }
}