using UI.Buttons.PageShopButtons;
using UnityEngine;

namespace UI.Screens
{
    public class ShopScreen : MonoBehaviour
    {
        [SerializeField] private PageShopButton[] _pageShopButtons;
        [SerializeField] private ShopPage[] _shopScreens;

        public virtual void Open()
        {
            gameObject.SetActive(true);
            ActivateShopButton(0);
            _shopScreens[0].Open(0);
        }

        public virtual void OpenPage(int index)
        {
            DeactivateShopPages();
            ActivateShopButton(index);
            _shopScreens[index].Open(0);
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
            foreach (var screen in _shopScreens)
                screen.Close();
        }
    }
}