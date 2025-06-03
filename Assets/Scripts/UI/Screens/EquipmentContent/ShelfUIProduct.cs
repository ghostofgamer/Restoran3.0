using Enums;
using Io.AppMetrica;
using SettingsContent.SoundContent;
using SoContent;
using TMPro;
using UnityEngine;

namespace UI.Screens.EquipmentContent
{
    public class ShelfUIProduct : EquipmentUIProduct
    {
        [SerializeField] private EquipmentType _equipmentType;
        [SerializeField] private ShelfConfigs _shelfConfigs;
        [SerializeField] private GameObject[] _shelf;
        [SerializeField] private TMP_Text _nameItem;
        [SerializeField] private GameObject _storage1;

        private int _currentBuyShelfIndex = -1;

        public override void Init(int levelPlayer)
        {
            _currentBuyShelfIndex = PlayerPrefs.GetInt("ShelfBuyed" + _equipmentType, -1);
            Initialization(levelPlayer);
        }

        public override void Buy()
        {
            int nextShelfIndex = _currentBuyShelfIndex + 1;

            if (nextShelfIndex < _shelfConfigs.shelves.Length)
            {
                if (_wallet.DollarValue.ToTotalCents() < CurrentPrice.ToTotalCents())
                {
                    SoundPlayer.Instance.PlayError();
                    Debug.Log("Не хватает денег ");
                    return;
                }
                
                AppMetrica.ReportEvent("Equipment", "{\"" + "Shelf" + "\":null}");
                SoundPlayer.Instance.PlayPayment();
                _wallet.Subtract(CurrentPrice);
                _shopScreen.MakePurchase();
                _currentBuyShelfIndex = nextShelfIndex;
                PlayerPrefs.SetInt("ShelfBuyed" + _equipmentType, _currentBuyShelfIndex);
                ActivateShelf(_currentBuyShelfIndex);
                _shopScreen.CloseScreen();
                Initialization(_currentBuyShelfIndex);
            }
            else
            {
                Debug.Log("Все шкафы уже куплены");
            }
        }

        public override void Initialization(int levelPlayer)
        {
            if (_currentBuyShelfIndex < 0)
            {
                _nameItem.text = _shelfConfigs.shelves[0].name;
                CurrentPrice = _shelfConfigs.shelves[0].price;
                _priceText.text = $"{CurrentPrice} ";
            }
            else if (_currentBuyShelfIndex + 1 < _shelfConfigs.shelves.Length)
            {
                _nameItem.text = _shelfConfigs.shelves[_currentBuyShelfIndex + 1].name;
                CurrentPrice = _shelfConfigs.shelves[_currentBuyShelfIndex + 1].price;
                _priceText.text = $"{CurrentPrice} ";

                if (_shelfConfigs.shelves[_currentBuyShelfIndex + 1].storage1ToUnlock)
                {
                    _requaredObjectInfo.SetActive(!_storage1.activeSelf);
                    _requaredText.text = $"Required  is storage 1";
                    _buyObjectInfo.SetActive(_storage1.activeSelf);
                }
            }
            else
            {
                _ownedObjectInfo.SetActive(true);
                _buyObjectInfo.SetActive(false);
                _requaredObjectInfo.SetActive(false);
                _nameItem.text = "Shelf";
                _priceText.text = "";
            }
        }

        private void ActivateShelf(int index)
        {
            _shelf[index].SetActive(true);
            Debug.Log("Activating shelf at index: " + index);
        }

        /*public override bool IsBuyed()
        {

        }*/
    }
}