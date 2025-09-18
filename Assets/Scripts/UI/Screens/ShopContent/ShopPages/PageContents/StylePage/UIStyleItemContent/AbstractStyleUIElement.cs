using Enums;
using UI.Buttons;
using UI.Screens.ShopContent.ShopPages.PageContents.StylePage.StyleScrollPageContents;
using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages.PageContents.StylePage.UIStyleItemContent
{
    public abstract class AbstractStyleUIElement : AbstractButton
    {
        [SerializeField]private StyleType _styleType;
        [SerializeField]protected StyleScrollPageContent StyleScrollPageContent; 
        [SerializeField] protected int Index;
        
        public StyleType StyleType => _styleType;
        public int IndexElement=> Index;
        
        public abstract void Init();

        public void SetIndex(int index)
        {
            Index = index;
        }
    }
}