using UI.Buttons.PageShopButtons;
using UnityEngine;

namespace UI.Screens
{
    public class ShopPage : MonoBehaviour
    {
        [SerializeField] private PageButton[] _pageButtons;
        
        public virtual void Open(int index)
        {
            gameObject.SetActive(true);
            DeactivatePagesButton();
            ActivatePageButton(index);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void DeactivatePagesButton()
        {
            foreach (var pageShopButton in _pageButtons)
                pageShopButton.DeactivateButton();
        }

        public void ActivatePageButton(int index)
        {
            DeactivatePagesButton();
            _pageButtons[index].ActivateButton();
        }
    }
}