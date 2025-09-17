using ADSContent;
using CustomizationContent;
using Enums;
using PlayerContent.LevelContent;
using SoContent.ShopStyleSOContent;
using UI.Screens.ShopContent.ShopPages.PageContents.StylePage.UIStyleItemContent.StyleContent;
using UnityEngine;
using WalletContent;

namespace UI.Screens.ShopContent.ShopPages.PageContents.StylePage.StyleScrollPageContents
{
    public class StyleScrollPageContent : PageScrollContent
    {
        [SerializeField] private Customization _customization;
        [SerializeField] private bool _multiChange;
        [SerializeField] private StyleUIElement[] _styleUIElements;
        [SerializeField] private Wallet _wallet;
        [SerializeField]private ADS _ads;
        [SerializeField]private PlayerLevel _playerLevel;
        [SerializeField]private StyleSoConfig _styleSoConfig;
        
        public Wallet Wallet => _wallet;
        public ADS Ads => _ads;
        public PlayerLevel PlayerLevel => _playerLevel;
        public StyleSoConfig StyleSoConfig => _styleSoConfig;

        public override void Init()
        {
            SetIndexes();

            foreach (var styleUIElement in _styleUIElements)
                styleUIElement.Init();
        }

        private void SetIndexes()
        {
            for (int i = 0; i < _styleUIElements.Length; i++)
                _styleUIElements[i].SetIndex(i);
        }

        public void ChangeCustomization(StyleType styleType, int index)
        {
            _customization.ChangeStyle(styleType, index);

            if (!_multiChange)
            {
                foreach (var styleUIElement in _styleUIElements)
                    styleUIElement.Deactivate();

                _styleUIElements[index].Activate();
            }
            else
            {
                if (_styleUIElements[index].IsActive)
                    _styleUIElements[index].Deactivate();
                else
                    _styleUIElements[index].Activate();
            }
        }

        public void PayStyle(int index)
        {
            _styleUIElements[index].Purchase();
            Init();
        }
    }
}