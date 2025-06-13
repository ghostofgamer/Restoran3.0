using System;
using Enums;
using I2.Loc;
using SettingsContent;
using SoContent;
using TMPro;
using TutorialContent;
using UnityEngine;
using UnityEngine.UI;
using WalletContent;

namespace UI.MenuUIContent
{
    public class DishesUIItem : AbstractUIMenuItem
    {
        private const string CurrentPriceKey = "CurrentPrice";

        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private ShopTutorialChanger _shopTutorialChanger;
        [SerializeField] private GameObject _openedContent;
        [SerializeField] private GameObject _closedContent;
        [SerializeField] private Slider _slider;
        [SerializeField] private Color _colorRed;
        [SerializeField] private Color _colorGreen;
        [SerializeField] private LanguageChanger _languageChanger;
        [SerializeField] private TMP_Text _priceDifferenceText;
        
        private bool _isFirstCall = true;
        private int _levelOpened;
        private DollarValue _purchasePrice;
        private DollarValue _minPrice;
        private DollarValue _maxPrice;
        private DollarValue _currentPrice;
        private DollarValue _recommendedPrice;

        public event Action<DollarValue, Color> ChangeCurrentPrice;
        public event Action<DollarValue> ChangeProfitPrice;

        public event Action<string, DollarValue> InitCompleted;

        private void OnEnable()
        {
            _languageChanger.LanguageChanged += InvokeRequiredTranslate;
        }

        private void OnDisable()
        {
            _languageChanger.LanguageChanged -= InvokeRequiredTranslate;
        }

        private void Start()
        {
            int totalCents = PlayerPrefs.GetInt(CurrentPriceKey + ItemConfig.ItemType, 0);

            if (totalCents > 0)
            {
                Debug.Log("INIT " + totalCents);

                _currentPrice = new DollarValue(0, 0).FromTotalCents(totalCents);
                UpdateProfitText();

                /*Color color = _currentPrice.ToTotalCents() <= _recommendedPrice.ToTotalCents()
                    ? _colorGreen
                    : _colorRed;*/

                ChangeCurrentPrice?.Invoke(_currentPrice, _colorGreen);

                if (ItemConfig != null)
                {
                    _levelOpened = ItemConfig.LevelOpened;
                    _purchasePrice = ItemConfig.PurchasePrice;
                    InitCompleted?.Invoke($"{LocalizationManager.GetTermTranslation("Required")} {_levelOpened}",
                        _purchasePrice);
                }
            }
        }

        private void InvokeRequiredTranslate()
        {
            InitCompleted?.Invoke($"{LocalizationManager.GetTermTranslation("Required")} {_levelOpened}",
                _purchasePrice);
        }

        public void AddItemToMenu()
        {
            if (_tutorial != null && _shopTutorialChanger != null &&
                _tutorial.CurrentType == TutorialType.LetsSetPrice)
            {
                _shopTutorialChanger.SetValueShopButton(true);
            }

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
                // InitCompleted?.Invoke($"Required is {_levelOpened} level", _purchasePrice);
                InitCompleted?.Invoke($"{LocalizationManager.GetTermTranslation("Required")} {_levelOpened}",
                    _purchasePrice);
                _recommendedPrice = ItemConfig.RecommendedPrice;

                if (PlayerPrefs.HasKey(CurrentPriceKey + ItemConfig.ItemType))
                {
                    int totalCents = PlayerPrefs.GetInt(CurrentPriceKey + ItemConfig.ItemType, 0);
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

                if (_tutorial != null && _shopTutorialChanger != null &&
                    _tutorial.CurrentType == TutorialType.LetsSetPrice)
                {
                }
                else
                {
                    OnSliderValueChanged(_slider.value);
                }
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
            Debug.Log(
                "OnSliderValueChangedOnSliderValueChangedOnSliderValueChangedOnSliderValueChangedOnSliderValueChanged");


            if (_tutorial != null && _shopTutorialChanger != null && !_isFirstCall)
            {
                if (_tutorial.CurrentType == TutorialType.LetsSetPrice)
                    _shopTutorialChanger.SetValueAddBurgerToMenuButton(true);
            }
            
            _isFirstCall = false;

            
            
            
            
            
            
            
            
            
            int totalCents = _minPrice.ToTotalCents() + (int)value;
            _currentPrice = new DollarValue(0, 0).FromTotalCents(totalCents);
            UpdateProfitText();

            int recommendedCents = _recommendedPrice.ToTotalCents();
            int maxCents = _maxPrice.ToTotalCents();
            int currentCents = _currentPrice.ToTotalCents();

            int difference = maxCents - recommendedCents;
            int step = difference / 9;

            int points = 0;

            if (currentCents > recommendedCents)
            {
                for (int i = 1; i <= 9; i++)
                {
                    int point = recommendedCents + step * i;
                    if (currentCents >= point)
                    {
                        points = i;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            _priceDifferenceText.text = points.ToString();

            // Color color = currentCents <= recommendedCents ? _colorGreen : _colorRed;

            ChangeCurrentPrice?.Invoke(_currentPrice, _colorGreen);
            PlayerPrefs.SetInt(CurrentPriceKey + ItemConfig.ItemType, totalCents);
            PlayerPrefs.Save();
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            /*int totalCents = _minPrice.ToTotalCents() + (int)value;
            _currentPrice = new DollarValue(0, 0).FromTotalCents(totalCents);
            UpdateProfitText();

            int recommendedCents = _recommendedPrice.ToTotalCents();
            int maxCents = _maxPrice.ToTotalCents();
            int currentCents = _currentPrice.ToTotalCents();

            int tenPercentAboveRecommended = (int)(recommendedCents * 1.10f);
            int difference = maxCents - tenPercentAboveRecommended;
            int step = difference / 5;

            int points = 0;

            if (currentCents > tenPercentAboveRecommended)
            {
                for (int i = 1; i <= 5; i++)
                {
                    int point = tenPercentAboveRecommended + step * i;
                    
                    if (currentCents >= point)
                    {
                        points = i;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            _priceDifferenceText.text = points.ToString(); // Обновляем текст в TMP_Text

            Color color = currentCents <= tenPercentAboveRecommended ? _colorGreen : _colorRed;

            ChangeCurrentPrice?.Invoke(_currentPrice, color);
            PlayerPrefs.SetInt(CurrentPriceKey + ItemConfig.ItemType, totalCents);
            PlayerPrefs.Save();*/

            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            /*int totalCents = _minPrice.ToTotalCents() + (int)value;
            _currentPrice = new DollarValue(0, 0).FromTotalCents(totalCents);
            UpdateProfitText();

            Color color = _currentPrice.ToTotalCents() <= _recommendedPrice.ToTotalCents() * 1.10
                ? _colorGreen
                : _colorRed;

            ChangeCurrentPrice?.Invoke(_currentPrice, color);
            PlayerPrefs.SetInt(CurrentPriceKey + ItemConfig.ItemType, totalCents);
            PlayerPrefs.Save();*/
        }
    }
}