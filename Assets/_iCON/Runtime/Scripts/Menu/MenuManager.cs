using System;
using System.Collections.Generic;
using CryStar.Attribute;
using CryStar.Utility;
using CryStar.Utility.Enum;
using iCON.Enums;
using iCON.UI;
using UnityEngine;

namespace iCON.Menu
{
    /// <summary>
    /// Menuを管理するクラス
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        /// <summary>
        /// View
        /// </summary>
        [SerializeField, HighlightIfNull]
        private InGameCanvasManager _view;
        
        /// <summary>
        /// メニューの状態
        /// </summary>
        [SerializeField]
        private MenuSystemState _currentState;
        
        /// <summary>
        /// 現在のステートのステートマシン用クラスの参照
        /// </summary>
        private MenuStateBase _currentStateHandler;
        
        /// <summary>
        /// Enumとステートマシン用クラスのkvpの辞書
        /// </summary>
        private Dictionary<MenuSystemState, MenuStateBase> _states;

        #region Life cycle

        private void Awake()
        {
            // ステートマシンの初期化
            InitializeStates();
            SetState(_currentState);
        }

        #endregion
        
        /// <summary>
        /// メニューの状態を変更する
        /// </summary>
        public void SetState(MenuSystemState state)
        {
            try
            {
                _currentStateHandler?.Exit();
                _currentState = state;
                _currentStateHandler = _states[state];
                _currentStateHandler.Enter(this, _view);
            }
            catch(Exception e)
            {
                LogUtility.Error($"メニュー操作中に例外が発生しました {e.Message}", LogCategory.Gameplay);
            }
        }
        
        /// <summary>
        /// StateMachineの初期化
        /// </summary>
        private void InitializeStates()
        {
            _states = new Dictionary<MenuSystemState, MenuStateBase>();
    
            try
            {
                _states.Add(MenuSystemState.MainMenu, new MainMenuState());
                _states.Add(MenuSystemState.CharacterStates, new CharacterStatusState());
                _states.Add(MenuSystemState.Item, new ItemState());
            }
            catch(Exception e)
            {
                LogUtility.Error($"State初期化中に例外が発生: {e.Message}", LogCategory.Gameplay);
            }
        }
    }
}

