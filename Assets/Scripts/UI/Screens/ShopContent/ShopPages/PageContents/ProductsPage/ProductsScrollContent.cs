using System;
using PlayerContent.LevelContent;
using UI.Screens.ShopContent.ItemUIProductContent;
using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage
{
    public class ProductsScrollContent : PageScrollContent
    {
        [SerializeField] private ItemCartScroll _itemCartScroll;
        [SerializeField] private ItemUIProduct[] _products;
        [SerializeField] private PlayerLevel _playerLevel;

        private void OnEnable()
        {
            _playerLevel.LevelChanged += UpdateRequiredLevelProducts;
        }

        private void OnDisable()
        {
            _playerLevel.LevelChanged -= UpdateRequiredLevelProducts;
        }

        public override void Init()
        {
            _itemCartScroll.ClearItems();

            UpdateRequiredLevelProducts(_playerLevel.CurrentLevel);
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

        private void UpdateRequiredLevelProducts(int level)
        {
            foreach (var product in _products)
                product.CheckUnlocked(level);
        }
    }
}