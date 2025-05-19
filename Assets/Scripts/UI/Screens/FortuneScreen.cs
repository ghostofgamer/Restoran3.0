using System;

namespace UI.Screens
{
    public class FortuneScreen : AbstractScreen
    {
        public event Action FortuneScreenClosed;
        
        public override void CloseScreen()
        {
            FortuneScreenClosed?.Invoke();
            base.CloseScreen();
        }
    }
}