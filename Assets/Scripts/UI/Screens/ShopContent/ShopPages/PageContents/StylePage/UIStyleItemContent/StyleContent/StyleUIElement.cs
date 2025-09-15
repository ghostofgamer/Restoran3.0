namespace UI.Screens.ShopContent.ShopPages.PageContents.StylePage.UIStyleItemContent.StyleContent
{
    public class StyleUIElement : AbstractStyleUIElement
    {
        public override void OnClick()
        {
            Change();
        }

        public override void Init()
        {
        }

        private void Change()
        {
            StyleScrollPageContent.ChangeCustomization(StyleType, Index);
        }
    }
}