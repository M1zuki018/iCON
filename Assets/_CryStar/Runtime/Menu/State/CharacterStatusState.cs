using CryStar.Menu.Enums;
using iCON.Enums;
using iCON.UI;

namespace CryStar.Menu
{
    /// <summary>
    /// キャラクターのステータス画面
    /// </summary>
    public class CharacterStatusState : MenuStateBase
    {
        public override void Enter(MenuManager manager, InGameCanvasManager view)
        {
            base.Enter(manager, view);
            view.PushCanvas(InGameCanvasType.CharacterStates);
            
            // TODO: キャラクターのステータス表示関連の処理を書く
        }

        public override void Back()
        {
            // メインメニューへ遷移
            MenuManager.SetState(MenuSystemState.MainMenu);
        }

        public override void Exit()
        {
            base.Exit();
            View.PopCanvas();
        }
    }
}