using UI.Buttons.PageShopButtons;
using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages
{
    public class ShopPage : MonoBehaviour
    {
        [SerializeField] private PageButton[] _pageButtons;
        [SerializeField] private PageContent[] _pageContents;
        
        public virtual void Open(int index)
        {
            gameObject.SetActive(true);
            DeactivatePages();
            ActivatePage(index);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void DeactivatePages()
        {
            foreach (var pageShopButton in _pageButtons)
                pageShopButton.DeactivateButton();

            foreach (var page in _pageContents)
                page.Close();
        }

        public void ActivatePage(int index)
        {
            DeactivatePages();
            _pageButtons[index].ActivateButton();
            _pageContents[index].Open();
        }
    }
}