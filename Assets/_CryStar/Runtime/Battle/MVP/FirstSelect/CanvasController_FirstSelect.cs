using System;
using CryStar.Attribute;
using CryStar.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace iCON.UI
{
    /// <summary>
    /// CanvasController_FirstSelect
    /// </summary>
    public class CanvasController_FirstSelect : WindowBase
    {
        [SerializeField, HighlightIfNull] 
        private FirstSelectView _view;
        
        private FirstSelectPresenter _presenter = new FirstSelectPresenter();

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