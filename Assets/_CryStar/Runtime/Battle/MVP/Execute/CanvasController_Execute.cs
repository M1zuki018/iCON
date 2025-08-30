using CryStar.Attribute;
using UnityEngine;

namespace CryStar.CommandBattle
{
    /// <summary>
    /// CanvasController_Execute
    /// </summary>
    public class CanvasController_Execute : WindowBase
    {
        /// <summary>
        /// View
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private ExecuteView _view;
        
        /// <summary>
        /// Presenter
        /// </summary>
        private ExecutePresenter _presenter = new ExecutePresenter();
        
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