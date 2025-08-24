using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShelfContent
{
    public class ShelfPositionViewer : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _valueText;
        
        public void Init(Sprite sprite, int value)
        {
            _image.gameObject.SetActive(true);
            _image.sprite = sprite;
            // _valueText.text = $"{value.ToString()}";
        }

        public void SetActiveValue(int value)
        {
            _valueText.text = $"{value.ToString()}";
        }

        public void Clear()
        {
            _image.gameObject.SetActive(false);
            _image.sprite = null;
            _valueText.text = "";
        }
    }
}