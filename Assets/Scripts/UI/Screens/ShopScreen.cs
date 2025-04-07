using UI.Buttons.PageShopButtons;
using UnityEngine;

namespace UI.Screens
{
    public class ShopScreen : MonoBehaviour
    {
        [SerializeField] private PageShopButton[] _pageShopButtons;
        // [SerializeField]private 

        public void Open()
        {
            ActivateShopButton(0);
        }

        public void OpenPage(int index)
        {
            ActivateShopButton(index);
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
    }
}