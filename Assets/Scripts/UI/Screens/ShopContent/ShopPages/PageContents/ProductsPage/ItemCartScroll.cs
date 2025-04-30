using Enums;
using UnityEngine;
using WalletContent;

namespace UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage
{
    public class ItemCartScroll : MonoBehaviour
    {
        [SerializeField] private ItemCart _prefabItemCart;
        [SerializeField] private Transform _container;

        private ItemCart[] _items;

        public void Clear()
        {
        }

        public void AddItemCart(ItemType itemType, int amount, DollarValue pricePerUnit, DollarValue totalPrice,
            string name)
        {
            ItemCart itemCart = Instantiate(_prefabItemCart, _container);
            itemCart.Init(itemType, amount, pricePerUnit, totalPrice, name);
        }
    }
}