using System;
using SoContent;
using UnityEngine;
using UnityEngine.UI;
using WalletContent;

namespace UI.MenuUIContent
{
    public class DishesUIItem : AbstractUIMenuItem
    {
        private const string CurrentPriceKey = "CurrentPrice";

        [SerializeField] private GameObject _openedContent;
        [SerializeField] private GameObject _closedContent;
        [SerializeField] private Slider _slider;
        [SerializeField] private Color _colorRed;

        private int _levelOpened;
        private DollarValue _purchasePrice;
        private DollarValue _minPrice;
        private DollarValue _maxPrice;
        private DollarValue _currentPrice;
        private DollarValue _recommendedPrice;

        public event Action<DollarValue,Color> ChangeCurrentPrice;
        public event Action<DollarValue> ChangeProfitPrice;

        public event Action<string, DollarValue> InitCompleted;

        public void AddItemToMenu()
        {
            _menuScrollContent.AddItem(ItemType);
        }

        public override void Init(ItemsConfig itemsConfig)
        {
            base.Init(itemsConfig);

            if (ItemConfig != null)
            {
                _levelOpened = ItemConfig.LevelOpened;
                _purchasePrice = ItemConfig.PurchasePrice;
                _minPrice = _purchasePrice;
                _maxPrice = ItemConfig.MaxPrice;
                InitCompleted?.Invoke($"Required is {_levelOpened} level", _purchasePrice);
                _recommendedPrice = ItemConfig.RecommendedPrice;

                if (PlayerPrefs.HasKey(CurrentPriceKey + ItemConfig.ItemType))
                {
                    int totalCents = PlayerPrefs.GetInt(CurrentPriceKey+ ItemConfig.ItemType,0);
                    _currentPrice = new DollarValue(0, 0).FromTotalCents(totalCents);
                }
                else
                {
                    _currentPrice = new DollarValue(_recommendedPrice.Dollars, _recommendedPrice.Cents);
                }
                
                UpdateProfitText();

                _slider.minValue = 0;
                _slider.maxValue = _maxPrice.ToTotalCents() - _minPrice.ToTotalCents();
                _slider.value = _currentPrice.ToTotalCents() - _minPrice.ToTotalCents();
                _slider.onValueChanged.AddListener(OnSliderValueChanged);
                OnSliderValueChanged(_slider.value);
            }
        }

        public void SetValue(int levelPlayer)
        {
            _openedContent.SetActive(levelPlayer >= _levelOpened);
            _closedContent.SetActive(levelPlayer < _levelOpened);
        }

        private void UpdateProfitText()
        {
            DollarValue p =
                new DollarValue(0, 0).FromTotalCents(_currentPrice.ToTotalCents() - _purchasePrice.ToTotalCents());

            ChangeProfitPrice?.Invoke(p);
        }

        private void OnSliderValueChanged(float value)
        {
            int totalCents = _minPrice.ToTotalCents() + (int)value;
            _currentPrice = new DollarValue(0, 0).FromTotalCents(totalCents);
            UpdateProfitText();
            
            Color color = _currentPrice.ToTotalCents() <= _recommendedPrice.ToTotalCents() * 1.10
                ? Color.green
                : _colorRed;
            
            ChangeCurrentPrice?.Invoke(_currentPrice,color);
            PlayerPrefs.SetInt(CurrentPriceKey + ItemConfig.ItemType, totalCents);
            PlayerPrefs.Save();
        }
    }
}