using SaveSystemContent;
using SoContent.ShopStyleSOContent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WalletContent;

namespace UI.Screens.ShopContent.ShopPages.PageContents.StylePage.UIStyleItemContent.StyleContent
{
    public class StyleUIElement : AbstractStyleUIElement
    {
        [SerializeField] private Button _payButton;
        [SerializeField] private Button _rewardButton;
        [SerializeField] private TMP_Text _requiredText;
        [SerializeField] private Image _activeImage;
        [SerializeField] private Image _payButtonImage;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Sprite[] _activeSprites;
        [SerializeField] private Color[] _payButtonColors;
        [SerializeField]private string _keyInap;

        private bool _openStart;
        private bool _isReward;
        private int _openLevel;
        private DollarValue _price;
        private bool _requiresZoneUnlock;
        private int _zoneIndex;
        private int _zoneFactor = 2;

        public bool IsActive { get; private set; } = false;
        public bool IsOwned { get; private set; } = false;

        public override void OnClick()
        {
            Change();
        }

        public override void Init()
        {
            StyleSoConfig styleSoConfig = StyleScrollPageContent.StyleSoConfig;

            if (styleSoConfig == null)
            {
                Debug.Log(" Style Config is NULL");
                return;
            }

            Load();
            InitializationConfig(styleSoConfig);
            SetRequiredText();
            SetColorPrice();

            _activeImage.sprite = _activeSprites[IsActive ? 0 : 1];
            SetValue();
        }

        private void InitializationConfig(StyleSoConfig styleSoConfig)
        {
            _openStart = styleSoConfig.StyleSoConfigElements[Index].IsOpenStart;
            _isReward = styleSoConfig.StyleSoConfigElements[Index].IsRewardStyle;
            _openLevel = styleSoConfig.StyleSoConfigElements[Index].OpenLevel;
            _price = styleSoConfig.StyleSoConfigElements[Index].DollarValue;
            _requiresZoneUnlock = styleSoConfig.StyleSoConfigElements[Index].RequiresZoneUnlock;
            _zoneIndex = styleSoConfig.StyleSoConfigElements[Index].ZoneIndex;
        }

        private void SetRequiredText()
        {
            _requiredText.text = _requiresZoneUnlock
                ? $"Required level: {_openLevel.ToString()} and Zone {_zoneIndex + _zoneFactor}"
                : $"Required level: {_openLevel.ToString()}";
        }

        private void SetColorPrice()
        {
            _priceText.text = _price.ToString();
            int walletCents = StyleScrollPageContent.Wallet.DollarValue.ToTotalCents();
            int priceCents = _price.ToTotalCents();
            _payButtonImage.color = walletCents < priceCents
                ? _payButtonColors[1]
                : _payButtonColors[0];
        }

        public void Purchase()
        {
            IsOwned = true;
            Save();
        }

        public void Activate()
        {
            IsActive = true;
            _activeImage.sprite = _activeSprites[0];
            Save();
        }

        public void Deactivate()
        {
            IsActive = false;
            _activeImage.sprite = _activeSprites[1];
            Save();
        }

        private void Change()
        {
            if (!IsOwned && !_openStart)
                return;

            StyleScrollPageContent.ChangeCustomization(StyleType, Index);
        }

        private void SetValue()
        {
            int value = PlayerPrefs.GetInt(_keyInap, 0);
            
            if (value > 0)
            {
                IsOwned = true;
                _payButton.gameObject.SetActive(false);
                _rewardButton.gameObject.SetActive(false);
                _requiredText.gameObject.SetActive(false);
                _activeImage.gameObject.SetActive(true);
                return;
            }
            
            if (IsOwned || _openStart)
            {
                _payButton.gameObject.SetActive(false);
                _rewardButton.gameObject.SetActive(false);
                _requiredText.gameObject.SetActive(false);
                _activeImage.gameObject.SetActive(true);
                return;
            }
            
            
            if (!_requiresZoneUnlock)
            {
                _payButton.gameObject.SetActive(!IsOwned && !_openStart && !_isReward &&
                                                _openLevel <= StyleScrollPageContent.PlayerLevel.CurrentLevel);
                _rewardButton.gameObject.SetActive(_isReward && !IsOwned &&
                                                   _openLevel <= StyleScrollPageContent.PlayerLevel.CurrentLevel);
                _requiredText.gameObject.SetActive(_openLevel > StyleScrollPageContent.PlayerLevel.CurrentLevel &&
                                                   !IsOwned);
                _activeImage.gameObject.SetActive(IsOwned || _openStart);
            }

            if (_requiresZoneUnlock)
            {
                _payButton.gameObject.SetActive(!IsOwned && !_openStart && !_isReward &&
                                                _openLevel <= StyleScrollPageContent.PlayerLevel.CurrentLevel &&
                                                !StyleScrollPageContent.ZonesRestaurant[_zoneIndex].activeSelf);
                _rewardButton.gameObject.SetActive(_isReward && !IsOwned &&
                                                   _openLevel <= StyleScrollPageContent.PlayerLevel.CurrentLevel &&
                                                   !StyleScrollPageContent.ZonesRestaurant[_zoneIndex].activeSelf);
                _requiredText.gameObject.SetActive(
                    !IsOwned && (
                        _openLevel > StyleScrollPageContent.PlayerLevel.CurrentLevel ||
                        StyleScrollPageContent.ZonesRestaurant[_zoneIndex].activeSelf
                    )
                );
                _activeImage.gameObject.SetActive(IsOwned || _openStart);
            }
        }

        public void Pay()
        {
            _priceText.text = _price.ToString();
            int walletCents = StyleScrollPageContent.Wallet.DollarValue.ToTotalCents();
            int priceCents = _price.ToTotalCents();

            if (walletCents < priceCents)
            {
                Debug.Log("Недостаточно средств");
                return;
            }

            Save();

            StyleScrollPageContent.Wallet.Subtract(_price);
            StyleScrollPageContent.PayStyle(Index);
        }

        public void RewardStyle()
        {
            StyleScrollPageContent.Ads.ShowRewarded(() => OpenStyle());
        }

        public void OpenStyle()
        {
            IsOwned = true;
        }

        public void Save()
        {
            var save = new StyleSaveData
            {
                IsOwnedInfo = IsOwned,
                IsActiveInfo = IsActive,
                IndexInfo = Index,
                StyleTypeInfo = StyleType.ToString()
            };

            SaveDataGame.SaveJson(StyleType.ToString() + Index, save);
        }

        public void Load()
        {
            StyleSaveData data = LoadDataGame.LoadJson<StyleSaveData>(StyleType.ToString() + Index);

            if (data != null)
            {
                IsOwned = data.IsOwnedInfo;
                IsActive = data.IsActiveInfo;
            }
            else
            {
                IsOwned = false;
                IsActive = false;
            }
        }

        public void DeleteSave()
        {
            string key = StyleType.ToString() + Index;
            SaveDataGame.DeleteJson(key);
            Debug.Log($"Deleted save for: {key}");
            IsOwned = false;
            IsActive = false;
            Init();
        }
    }

    [System.Serializable]
    public class StyleSaveData
    {
        public bool IsOwnedInfo;
        public bool IsActiveInfo;
        public int IndexInfo;
        public string StyleTypeInfo;
    }
}