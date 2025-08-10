using iCON.UI;

namespace iCON.Menu
{
    public abstract class MenuStateBase
    {
        protected MenuManager MenuManager;
        protected InGameCanvasManager View;

        public virtual void Enter(MenuManager manager, InGameCanvasManager view)
        {
            View = view;
        }
        
        public virtual void Exit(){}
    }
}
