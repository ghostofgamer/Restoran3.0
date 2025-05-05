using TMPro;
using UI.Screens.ShopContent;
using UnityEngine;
using WalletContent;

namespace UI.Screens
{
    public class EquipmentUIProduct : MonoBehaviour
    {
        public const string Equipment = "Equipment";

        [SerializeField] private GameObject _ownedObjectInfo;
        [SerializeField] private GameObject _requaredObjectInfo;
        [SerializeField] private GameObject _buyObjectInfo;
        [SerializeField] private int _levelOpened;
        [SerializeField] private GameObject _equipment;
        [SerializeField] private TMP_Text _requaredText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private int _dollarPrice;
        [SerializeField] private int _centPrice;
        [SerializeField] private ShopScreen _shopScreen;

        private bool _isOwned;
        private DollarValue _currentPrice;

        private void Start()
        {
            _currentPrice = new DollarValue(_dollarPrice, _centPrice);
            _requaredText.text = $"Requared is {_levelOpened} level";
            _priceText.text = $"{_currentPrice.ToString()} ";
        }

        public void Init(int levelPlayer)
        {
            _isOwned = IsBuyed();
            _requaredObjectInfo.SetActive(levelPlayer < _levelOpened && !_isOwned);
            _ownedObjectInfo.SetActive(levelPlayer >= _levelOpened && _isOwned);
            _buyObjectInfo.SetActive(levelPlayer >= _levelOpened && !_isOwned);
        }

        public void Buy()
        {
            _isOwned = true;
            _ownedObjectInfo.SetActive(true);
            _buyObjectInfo.SetActive(false);
            _equipment.gameObject.SetActive(true);
            PlayerPrefs.SetInt(Equipment + _levelOpened, 1);
            _shopScreen.CloseScreen();
        }

        public bool IsBuyed()
        {
            return PlayerPrefs.GetInt(Equipment + _levelOpened, 0) > 0;
        }
    }
}