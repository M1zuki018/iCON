using iCON.Battle;
using UnityEngine;

public class FirstSelectPresenter
{
    private FirstSelectView _view;
    private FirstSelectModel _model;
    
    public void Setup(FirstSelectView view)
    {
        _view = view;
        _model = new FirstSelectModel();
        
        _model.Setup();
        _view.Setup(_model.StartBattle, _model.TryEscape);
    }

    public void Exit()
    {
        _view?.Exit();
    }
}
