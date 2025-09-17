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

        private bool _openStart;
        private bool _isReward;
        private int _openLevel;
        private DollarValue _price;

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

            _openStart = styleSoConfig.StyleSoConfigElements[Index].IsOpenStart;
            _isReward = styleSoConfig.StyleSoConfigElements[Index].IsRewardStyle;
            _openLevel = styleSoConfig.StyleSoConfigElements[Index].OpenLevel;
            _price = styleSoConfig.StyleSoConfigElements[Index].DollarValue;

            _requiredText.text = $"Required level: {_openLevel.ToString()}";
            _priceText.text = _price.ToString();
            int walletCents = StyleScrollPageContent.Wallet.DollarValue.ToTotalCents();
            int priceCents = _price.ToTotalCents();
            _payButtonImage.color = walletCents < priceCents
                ? _payButtonColors[1]
                : _payButtonColors[0];

            _activeImage.sprite = _activeSprites[IsActive ? 0 : 1];
            SetValue();
        }

        public void Purchase()
        {
            IsOwned = true;
        }

        public void Activate()
        {
            IsActive = true;
            _activeImage.sprite = _activeSprites[0];
        }

        public void Deactivate()
        {
            IsActive = false;
            _activeImage.sprite = _activeSprites[1];
        }

        private void Change()
        {
            if (!IsOwned && !_openStart)
                return;

            StyleScrollPageContent.ChangeCustomization(StyleType, Index);
        }

        private void SetValue()
        {
            _payButton.gameObject.SetActive(!IsOwned && !_openStart && !_isReward &&
                                            _openLevel <= StyleScrollPageContent.PlayerLevel.CurrentLevel);
            _rewardButton.gameObject.SetActive(_isReward && !IsOwned &&
                                               _openLevel <= StyleScrollPageContent.PlayerLevel.CurrentLevel);
            _requiredText.gameObject.SetActive(_openLevel > StyleScrollPageContent.PlayerLevel.CurrentLevel);
            _activeImage.gameObject.SetActive(IsOwned || _openStart);
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

            StyleScrollPageContent.Wallet.Subtract(_price);
            StyleScrollPageContent.PayStyle(Index);
        }

        public void RewardStyle()
        {
            StyleScrollPageContent.Ads.ShowRewarded(() => { StyleScrollPageContent.PayStyle(Index); });
        }
    }
}