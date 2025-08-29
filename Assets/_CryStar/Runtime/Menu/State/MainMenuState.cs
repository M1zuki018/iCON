using CryStar.Core;
using CryStar.Data.Scene;
using Cysharp.Threading.Tasks;
using iCON.Enums;
using iCON.System;
using iCON.UI;
using UnityEngine;

namespace iCON.Menu
{
    /// <summary>
    /// メインメニュー
    /// </summary>
    public class MainMenuState : MenuStateBase
    {
        /// <summary>
        /// Canvas Controller
        /// </summary>
        private CanvasController_Menu _cc;
        
        public override void Enter(MenuManager manager, InGameCanvasManager view)
        {
            base.Enter(manager, view);
            
            // MainMenuのみを表示する
            view.ShowCanvas(InGameCanvasType.MainMenu);

            if (_cc == null)
            {
                // キャンバスコントローラーを取得
                _cc = view.CurrentCanvas as CanvasController_Menu;
            }

            if (_cc == null)
            {
                Debug.LogError("キャンバスが見つかりませんでした");
                return;
            }

            _cc.OnStatusButtonClicked += HandleStatusButton;
            _cc.OnItemButtonClicked += HandleItemButton;
            _cc.OnBackTitleButtonClicked += HandleBackTitleButton;
        }

        public override void Back()
        {
            // メニュー画面を閉じる
            View.CurrentCanvas.Hide();
            MenuManager.SetState(MenuSystemState.None);
        }

        public override void Exit()
        {
            if (_cc != null)
            {
                return;
            }
            
            _cc.OnStatusButtonClicked -= HandleStatusButton;
            _cc.OnItemButtonClicked -= HandleItemButton;
            _cc.OnBackTitleButtonClicked -= HandleBackTitleButton;
            base.Exit();
        }

        /// <summary>
        /// キャラクターステータスボタンを押したときの処理
        /// </summary>
        private void HandleStatusButton()
        {
            MenuManager.SetState(MenuSystemState.CharacterStates);
        }

        /// <summary>
        /// アイテムボタンを押したときの処理
        /// </summary>
        private void HandleItemButton()
        {
            MenuManager.SetState(MenuSystemState.Item);
        }

        private void HandleBackTitleButton()
        {
            var sceneLoader = ServiceLocator.GetGlobal<SceneLoader>();
            sceneLoader.LoadSceneAsync(new SceneTransitionData(SceneType.Title, true, true)).Forget();
        }
    }
}