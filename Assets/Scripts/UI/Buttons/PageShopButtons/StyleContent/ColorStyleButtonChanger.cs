using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Buttons.PageShopButtons.StyleContent
{
    public class ColorStyleButtonChanger : MonoBehaviour
    {
        [SerializeField] private Color _onBackGroundColor;
        [SerializeField] private Color _offBackGroundColor;
        [SerializeField] private Color _onTextColor;
        [SerializeField] private Color _offTextColor;
        [SerializeField]private Image _backGroundImage;
        [SerializeField]private TMP_Text _text;

        public void Deactivate()
        {
            _backGroundImage.color = _offBackGroundColor;
            _text.color = _offTextColor;
        }

        public void Activate()
        {
            _backGroundImage.color = _onBackGroundColor;
            _text.color = _onTextColor;
        }
    }
}