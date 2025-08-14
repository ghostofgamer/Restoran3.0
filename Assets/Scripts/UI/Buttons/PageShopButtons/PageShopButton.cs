using SettingsContent.SoundContent;
using TMPro;
using UI.Screens;
using UI.Screens.ShopContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons.PageShopButtons
{
    public class PageShopButton : AbstractButton
    {
        [SerializeField] private int _index;
        [SerializeField] private Color _activeButtonColor;
        [SerializeField] private Color _notActiveButtonColor;
        [SerializeField] private Image _imageButton;
        [SerializeField] private ShopScreen _shopScreen;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private TMP_Text _buttonText;

        protected int Index => _index;

        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _shopScreen.OpenPage(_index);
        }

        public void ActivateButton()
        {
            ChangeColorButton(_activeButtonColor);
            ChangeTextAndSpriteColor(_notActiveButtonColor);
        }

        public void DeactivateButton()
        {
            ChangeColorButton(_notActiveButtonColor);
            ChangeTextAndSpriteColor(_activeButtonColor);
        }

        private void ChangeColorButton(Color color)
        {
            _imageButton.color = color;
        }

        private void ChangeTextAndSpriteColor(Color color)
        {
            if (_buttonImage != null)
                _buttonImage.color = color;

            if (_buttonText != null)
                _buttonText.color = color;
        }
    }
}