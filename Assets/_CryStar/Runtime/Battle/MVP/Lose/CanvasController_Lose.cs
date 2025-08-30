using CryStar.Attribute;
using CryStar.CommandBattle;
using UnityEngine;

namespace iCON.UI
{
    /// <summary>
    /// CanvasController_Lose
    /// </summary>
    public class CanvasController_Lose : WindowBase
    {
        /// <summary>
        /// View
        /// </summary>
        [SerializeField, HighlightIfNull] 
        private LoseView _view;
        
        /// <summary>
        /// Presenter
        /// </summary>
        private LosePresenter _presenter = new LosePresenter();
        
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