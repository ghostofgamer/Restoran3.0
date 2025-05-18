using SettingsContent.SoundContent;
using TMPro;
using UI.Screens.ShopContent;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private Wallet _wallet;
        [SerializeField] private Color _activeButtonColor;
        [SerializeField] private Color _notActiveButtonColor;
        [SerializeField] private Image _buyButtonImage;

        private bool _isOwned;
        private DollarValue _currentPrice;

        public void Init(int levelPlayer)
        {
            _isOwned = IsBuyed();
            _currentPrice = new DollarValue(_dollarPrice, _centPrice);
            _requaredText.text = $"Requared is {_levelOpened} level";
            _priceText.text = $"{_currentPrice.ToString()} ";
            
            _requaredObjectInfo.SetActive(levelPlayer < _levelOpened && !_isOwned);
            _ownedObjectInfo.SetActive(levelPlayer >= _levelOpened && _isOwned);
            _buyObjectInfo.SetActive(levelPlayer >= _levelOpened && !_isOwned);
            
            _buyButtonImage.color = _wallet.DollarValue.ToTotalCents() >= _currentPrice.ToTotalCents()
                ? _activeButtonColor
                : _notActiveButtonColor;
        }

        public void Buy()
        {
            if (_wallet.DollarValue.ToTotalCents() < _currentPrice.ToTotalCents())
            {
                SoundPlayer.Instance.PlayError();
                Debug.Log("Не хватает денег ");
                return;
            }
            
            SoundPlayer.Instance.PlayPayment();
            _wallet.Subtract(_currentPrice);
            _shopScreen.MakePurchase();
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