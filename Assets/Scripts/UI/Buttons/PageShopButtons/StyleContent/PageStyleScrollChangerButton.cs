using UI.Screens.ShopContent.ShopPages.PageContents.StylePage;
using UnityEngine;

namespace UI.Buttons.PageShopButtons.StyleContent
{
    public class PageStyleScrollChangerButton : AbstractButton
    {
        [SerializeField] private StyleScrollContent _styleScrollContent;
        [SerializeField] private int _index;
        
        public override void OnClick()
        {
            _styleScrollContent.ChangeStyleScrollPage(_index);
        }
    }
}