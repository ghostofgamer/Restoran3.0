using System;
using Enums;
using ItemContent;
using SoContent;
using TMPro;
using UnityEngine;
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

        private DollarValue _pricePerUnit;
        private int _amountProduct = 1;
        private DollarValue _totalPrice;

        public event Action<int, DollarValue> AmountChanged;

        private void Start()
        {
            Ingredient ingredient = _ingredientsConfig.GetIngredient(_itemType);
            _pricePerUnit  = new DollarValue(ingredient.dollarsPrice, ingredient.centsPrice);
            _icon.sprite = ingredient.shopItemSprite;
            _nameItemUIProduct.text = ingredient.name;
            Debug.Log("A" + ingredient.itemType.ToString());
            AmountChanged?.Invoke(_amountProduct, _pricePerUnit);
        }

        public void IncreaseAmount()
        {
            if (_amountProduct >= 9)
                return;

            _amountProduct++;
            ChangeTotalPrice();
            AmountChanged?.Invoke(_amountProduct, _totalPrice);
        }

        public void DecreaseAmount()
        {
            if (_amountProduct > 1)
            {
                _amountProduct--;
                ChangeTotalPrice();
                AmountChanged?.Invoke(_amountProduct, _totalPrice);
            }
        }

        public void CheckUnlocked(bool value)
        {
            _lockContent.SetActive(value);
            _unlockContent.SetActive(!value);
        }

        public void AddItemToCart()
        {
            
        }

        private void ChangeTotalPrice()
        {
            int totalCents = _pricePerUnit.ToTotalCents(_pricePerUnit) * _amountProduct;
            Debug.Log("Total Cents: " + totalCents);
            _totalPrice = _pricePerUnit.FromTotalCents(totalCents);
            Debug.Log("Total Price: " + _totalPrice.ToString());
        }
    }
}