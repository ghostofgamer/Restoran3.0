using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage
{
    public class ProductsScrollContent : PageScrollContent
    {
        [SerializeField] private ShoppingCartScroll _shoppingCartScroll;
        
        public override void Init()
        {
            _shoppingCartScroll.Clear();
            
            Debug.Log("GasmeObj " + gameObject.name);
        }
    }
}