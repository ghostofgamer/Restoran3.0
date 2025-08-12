using TMPro;
using UnityEngine;
using WalletContent;

namespace UI
{
    public class FlyValue : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private bool _isEnergy;

        private Color _color;

        public void ShowFly(DollarValue dollarValue, bool profitValue)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            // _text.color = profitValue ? Color.green : Color.red;
            _text.color = _isEnergy ? (profitValue ? Color.yellow : Color.red) : (profitValue ? Color.green : Color.red);

            _text.text = dollarValue.ToString();
        }

        public void ShowFly(int value)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            
            // _text.color = value >= 0 ? Color.green : Color.red;
            _text.color = _isEnergy ? (value >= 0 ? Color.yellow : Color.red) : (value >= 0 ? Color.green : Color.red);
            _text.text = value >= 0 ? $"+{value.ToString()}" : $"{value.ToString()}";
        }
    }
}