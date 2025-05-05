using RestaurantContent;
using TMPro;
using UnityEngine;
using WalletContent;

namespace UI.Screens.ShopContent
{
    public class PlaceUIProduct : MonoBehaviour
    {
        [SerializeField] private GameObject _ownedObjectInfo;
        [SerializeField] private GameObject _requaredObjectInfo;
        [SerializeField] private int _index;
        [SerializeField] private GameObject _buyObjectInfo;
        [SerializeField] private ZoneUIProduct _zoneProduct;
        [SerializeField] private TMP_Text _requaredText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private ShopScreen _shopScreen;
        [SerializeField] private int _dollars;
        [SerializeField] private int _cents;
        [SerializeField] private PlaceTable _placeTable;
        
        private DollarValue _dollarValue;

        public bool IsOwned { get; private set; }

        private void Start()
        {
            _dollarValue = new DollarValue(_dollars, _cents);
            _requaredText.text = _zoneProduct != null
                ? $"Requared is  zone"
                : $"Requared";
            _priceText.text = $"{_dollarValue.ToString()} ";
        }

        public void Init()
        {
            IsOwned = IsBuyed();

            if (_zoneProduct == null)
            {
                SetValue(false, IsOwned, !IsOwned);
            }
            else
            {
                SetValue(!_zoneProduct.IsBuyed(),
                    _zoneProduct.IsBuyed() && IsOwned,
                    _zoneProduct.IsBuyed() && !IsOwned);
            }
        }

        public bool IsBuyed()
        {
            return PlayerPrefs.GetInt("Place" + _index, 0) > 0;
        }

        public void Buy()
        {
            IsOwned = true;
            _ownedObjectInfo.SetActive(true);
            _buyObjectInfo.SetActive(false);
            // _wallZone.gameObject.SetActive(false);
            PlayerPrefs.SetInt("Place" + _index, 1);
            _shopScreen.CloseScreen();
            _placeTable.Activate();
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