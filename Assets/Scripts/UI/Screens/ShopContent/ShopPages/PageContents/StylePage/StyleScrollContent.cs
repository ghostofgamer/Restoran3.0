using UI.Buttons.PageShopButtons.StyleContent;
using UI.Screens.ShopContent.ShopPages.PageContents.StylePage.StyleScrollPageContents;
using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages.PageContents.StylePage
{
    public class StyleScrollContent : PageScrollContent
    {
        [SerializeField] private StyleScrollPageContent[] _scrollStylePages;
        [SerializeField]private ColorStyleButtonChanger[] _colorStyleButtonChangers;
        
        public override void Init()
        {
            ChangeStyleScrollPage(0);
        }

        public void ChangeStyleScrollPage(int index)
        {
            DeactivationStylePages();
            _scrollStylePages[index].Activate();
            _scrollStylePages[index].Init();
            _colorStyleButtonChangers[index].Activate();
        }

        private void DeactivationStylePages()
        {
            foreach (var scrollStylePage in _scrollStylePages)
                scrollStylePage.Deactivate();
            
            foreach (var colorStyleButtonChanger in _colorStyleButtonChangers)
                colorStyleButtonChanger.Deactivate();
        }
    }
}