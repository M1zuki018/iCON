namespace CryStar.CommandBattle
{
    /// <summary>
    /// TryEscape_Presenter
    /// </summary>
    public class TryEscapePresenter
    {
        private TryEscapeView _view;
        private TryEscapeModel _model;
    
        /// <summary>
        /// Setup（Enterのタイミングで呼び出し）
        /// </summary>
        public void Setup(TryEscapeView view)
        {
            _view = view;
            _model = new TryEscapeModel();
            
            _model.Setup();
            _view.Setup();
        }

        /// <summary>
        /// Exit
        /// </summary>
        public void Exit()
        {
            _view?.Exit();
        }
    }
}