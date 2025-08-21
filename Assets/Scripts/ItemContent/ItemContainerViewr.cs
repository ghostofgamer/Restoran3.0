using InteractableContent;
using TMPro;
using UnityEngine;

namespace ItemContent
{
    public class ItemContainerViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private ItemContainer _itemContainer;

        private void OnEnable()
        {
            _itemContainer.ValueChanged += ShowValue;
        }

        private void OnDisable()
        {
            _itemContainer.ValueChanged -= ShowValue;
        }

        private void ShowValue(int currentValue, int maxValue)
        {
            _valueText.text = $"{currentValue}/{maxValue}";
        }
    }
}