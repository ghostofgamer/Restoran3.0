using UI.Buttons.PageShopButtons;
using UnityEngine;

namespace UI.Buttons
{
    public class GoDishesEquipmentPageButton : AbstractButton
    {
        [SerializeField]private PageShopButton _pageShopButtons;
        [SerializeField] private PageButton _pageButton;
        
        public override void OnClick()
        {
            _pageShopButtons.OnClick();
            _pageButton.OnClick();
        }
    }
}