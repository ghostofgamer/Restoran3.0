using TMPro;
using UnityEngine;
using WalletContent;

namespace UI.Screens.ShopContent
{
    public class ZoneUIProduct : MonoBehaviour
    {
        [SerializeField] private GameObject _ownedObjectInfo;
        [SerializeField] private GameObject _requaredObjectInfo;
        [SerializeField] private GameObject _buyObjectInfo;
        [SerializeField] private int _levelOpened;
        [SerializeField] private GameObject _wallZone;
        [SerializeField] private ZoneUIProduct _previousWallZone;
        [SerializeField] private TMP_Text _requaredText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private int _dollars;
        [SerializeField] private int _cents;
        [SerializeField] private bool _isFirstZone;
        [SerializeField] private ShopScreen _shopScreen;

        private DollarValue _dollarValue;

        public bool IsOwned { get; private set; }

        private void Start()
        {
            _dollarValue = new DollarValue(_dollars, _cents);
            _requaredText.text = _isFirstZone
                ? $"Requared is {_levelOpened} level"
                : $"Requared is {_levelOpened} level and prev zone";
            _priceText.text = $"{_dollarValue.ToString()} ";
        }

        public void Init(int levelPlayer)
        {
            IsOwned = IsBuyed();

            if (_isFirstZone)
            {
                SetValue(levelPlayer < _levelOpened && !IsOwned,
                    levelPlayer >= _levelOpened && IsOwned,
                    levelPlayer >= _levelOpened && !IsOwned);
            }
            else
            {
                SetValue((!IsOwned && !_previousWallZone.IsOwned),
                    levelPlayer >= _levelOpened && IsOwned && _previousWallZone.IsOwned,
                    levelPlayer >= _levelOpened && !IsOwned && _previousWallZone.IsOwned);
            }
        }

        public bool IsBuyed()
        {
            return PlayerPrefs.GetInt("Zona" + _levelOpened, 0) > 0;
        }

        public void Buy()
        {
            IsOwned = true;
            _ownedObjectInfo.SetActive(true);
            _buyObjectInfo.SetActive(false);
            _wallZone.gameObject.SetActive(false);
            PlayerPrefs.SetInt("Zona" + _levelOpened, 1);
            _shopScreen.CloseScreen();
        }

        private void SetValue(bool requaredObjectValue, bool ownedObjectValue, bool buyObjectValue)
        {
            Debug.Log("requaredObjectValue " + requaredObjectValue);
            Debug.Log("ownedObjectValue " + ownedObjectValue);
            Debug.Log("buyObjectValue " + buyObjectValue);

            _requaredObjectInfo.SetActive(requaredObjectValue);
            _ownedObjectInfo.SetActive(ownedObjectValue);
            _buyObjectInfo.SetActive(buyObjectValue);
        }
    }
}