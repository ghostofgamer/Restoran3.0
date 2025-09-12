
namespace UI.Screens.ShopContent.ShopPages.PageContents.StylePage.StyleScrollPageContents
{
    public class StyleScrollPageContent : PageScrollContent
    {
        public override void Init()
        {
        }

        public virtual void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}