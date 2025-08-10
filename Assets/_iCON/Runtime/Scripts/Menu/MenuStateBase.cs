using iCON.UI;

namespace iCON.Menu
{
    public abstract class MenuStateBase
    {
        protected MenuManager MenuManager;
        protected InGameCanvasManager View;

        public virtual void Enter(MenuManager manager, InGameCanvasManager view)
        {
            MenuManager = manager;
            View = view;
        }
        
        public virtual void Back(){}
        
        public virtual void Exit(){}
    }
}
