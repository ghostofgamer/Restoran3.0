using SaveSystemContent;
using UI.Buttons;
using UI.Screens.ShopContent.ShopPages.PageContents.StylePage.UIStyleItemContent.StyleContent;
using UnityEngine;

namespace Test
{
    public class DeleteStyleSave : AbstractButton
    {
        [SerializeField] private StyleUIElement[] styleUIElements;

        public override void OnClick()
        {
            foreach (var styleUIElement in styleUIElements)
                styleUIElement.DeleteSave();
        }
    }
}