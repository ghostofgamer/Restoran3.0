using System;
using Enums;
using ItemContent;
using SoContent;
using TMPro;
using UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WalletContent;

namespace UI.Screens.ShopContent.ItemUIProductContent
{
    public class ItemUIProduct : MonoBehaviour
    {
        [SerializeField] private IngredientsConfig _ingredientsConfig;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private int _minLevelToUnlock;
        [SerializeField] private GameObject _lockContent;
        [SerializeField] private GameObject _unlockContent;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameItemUIProduct;
        [SerializeField] private ProductsScrollContent _productsScrollContent;

        public event Action<int, DollarValue> AmountChanged;

        public DollarValue PricePerUnit { get; private set; }

        public int AmountProduct { get; private set; } = 1;

        public DollarValue TotalPrice { get; private set; }

        public string Name { get; private set; }

        public ItemType ItemType => _itemType;

        private void Start()
        {
            Ingredient ingredient = _ingredientsConfig.GetIngredient(_itemType);
            PricePerUnit = new DollarValue(ingredient.dollarsPrice, ingredient.centsPrice);
            _icon.sprite = ingredient.shopItemSprite;
            Name = ingredient.name;
            _nameItemUIProduct.text = Name;
            Debug.Log("A" + ingredient.itemType.ToString());
            TotalPrice = PricePerUnit;
            AmountChanged?.Invoke(AmountProduct, PricePerUnit);
        }

        public void IncreaseAmount()
        {
            if (AmountProduct >= 9)
                return;

            AmountProduct++;
            ChangeTotalPrice();
            AmountChanged?.Invoke(AmountProduct, TotalPrice);
        }

        public void DecreaseAmount()
        {
            if (AmountProduct > 1)
            {
                AmountProduct--;
                ChangeTotalPrice();
                AmountChanged?.Invoke(AmountProduct, TotalPrice);
            }
        }

        public void CheckUnlocked(bool value)
        {
            _lockContent.SetActive(value);
            _unlockContent.SetActive(!value);
        }

        public void AddItemToCart()
        {
            _productsScrollContent.AddItem(this);
        }

        private void ChangeTotalPrice()
        {
            int totalCents = PricePerUnit.ToTotalCents(PricePerUnit) * AmountProduct;
            TotalPrice = PricePerUnit.FromTotalCents(totalCents);
        }
    }
}