using UI.Screens.ShopContent.ItemUIProductContent;
using UnityEngine;

namespace UI.Buttons
{
    public class AddItemsCartButton : AbstractButton
    {
        [SerializeField] private ItemUIProduct _itemUIProduct;
        
        public override void OnClick()
        {
            _itemUIProduct.AddItemToCart();
        }
    }
}