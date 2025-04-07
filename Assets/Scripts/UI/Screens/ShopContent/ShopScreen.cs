using UI.Buttons.PageShopButtons;
using UI.Screens.ShopContent.ShopPages;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Screens.ShopContent
{
    public class ShopScreen : AbstractScreen
    {
        [SerializeField] private PageShopButton[] _pageShopButtons;
        [SerializeField] private ShopPage[] _shopPages;

        public override void OpenScreen()
        {
            base.OpenScreen();
            ActivateShopButton(0);
            OpenPage(0);
        }

        public virtual void OpenPage(int index)
        {
            DeactivateShopPages();
            ActivateShopButton(index);
            _shopPages[index].Open(0);
        }
        
        private void ActivateShopButton(int index)
        {
            DeactivateShopButtons();
            _pageShopButtons[index].ActivateButton();
        }
        
        private void DeactivateShopButtons()
        {
            foreach (var pageShopButton in _pageShopButtons)
                pageShopButton.DeactivateButton();
        }

        private void DeactivateShopPages()
        {
            foreach (var screen in _shopPages)
                screen.Close();
        }
    }
}