using CustomizationContent;
using Enums;
using UI.Screens.ShopContent.ShopPages.PageContents.StylePage.UIStyleItemContent.StyleContent;
using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages.PageContents.StylePage.StyleScrollPageContents
{
    public class StyleScrollPageContent : PageScrollContent
    {
        [SerializeField] private Customization _customization;
        [SerializeField] private bool _multiChange;
        [SerializeField] private StyleUIElement[] _styleUIElements;

        public override void Init()
        {
            SetIndexes();
        }

        private void SetIndexes()
        {
            for (int i = 0; i < _styleUIElements.Length; i++)
                _styleUIElements[i].SetIndex(i);
        }

        public void ChangeCustomization(StyleType styleType, int index)
        {
            _customization.ChangeStyle(styleType, index);
        }
    }
}