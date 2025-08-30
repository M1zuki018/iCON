using CryStar.Attribute;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// CanvasController_CommandSelect
    /// </summary>
    public class CanvasController_CommandSelect : WindowBase
    {
        /// <summary>
        /// View
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private CommandSelectView _view;
        
        /// <summary>
        /// Presenter
        /// </summary>
        private CommandSelectPresenter _presenter = new CommandSelectPresenter();
        
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