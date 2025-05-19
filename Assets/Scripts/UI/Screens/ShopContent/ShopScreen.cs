using Enums;
using PlayerContent.LevelContent;
using TutorialContent;
using UI.Buttons.PageShopButtons;
using UI.Screens.ShopContent.ShopPages;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.ShopContent
{
    public class ShopScreen : AbstractScreen
    {
        [SerializeField] private PageShopButton[] _pageShopButtons;
        [SerializeField] private ShopPage[] _shopPages;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Tutorial _tutorial;

        public override void OpenScreen()
        {
            base.OpenScreen();
            ActivateShopButton(0);
            OpenPage(0);
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
        }

        public virtual void OpenPage(int index)
        {
            DeactivateShopPages();
            ActivateShopButton(index);
            _shopPages[index].Open(0);
        }

        private void ActivateShopButton(int index)
        {
            if (_tutorial.CurrentType == TutorialType.OrderBurgerPatties)
            {
                SetInteractableButton(false);
            }
            else
            {
                SetInteractableButton(true);
                DeactivateShopButtons();
                _pageShopButtons[index].ActivateButton();
            }
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

        public void MakePurchase()
        {
            _playerLevel.AddExp(5);
        }

        private void SetInteractableButton(bool value)
        {
            foreach (var pageShopButton in _pageShopButtons)
                pageShopButton.GetComponent<Button>().interactable = value;
        }
    }
}