using UI.Screens.ShopContent.ItemUIProductContent;
using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage
{
    public class ProductsScrollContent : PageScrollContent
    {
        [SerializeField] private ItemCartScroll _itemCartScroll;

        public override void Init()
        {
            _itemCartScroll.ClearItems();

            Debug.Log("GasmeObj " + gameObject.name);
        }

        public void AddItem(ItemUIProduct itemUIProduct)
        {
            _itemCartScroll.AddItemCart(
                itemUIProduct.ItemType,
                itemUIProduct.AmountProduct,
                itemUIProduct.PricePerUnit,
                itemUIProduct.TotalPrice,
                itemUIProduct.Name);
        }
    }
}