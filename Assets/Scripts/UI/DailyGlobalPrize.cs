using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DailyGlobalPrize : MonoBehaviour
    {
        [SerializeField] private Image _prizeImage;
        [SerializeField] private TMP_Text _value;

        public void SetValue(Sprite sprite, int value)
        {
            _prizeImage.sprite = sprite;
            _value.text = value.ToString();
        }
    }
}