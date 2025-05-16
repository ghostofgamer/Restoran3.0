using System;
using InteractableContent;
using UnityEngine;

namespace SaveContent
{
    public class ItemContainerSaver : MonoBehaviour
    {
        private ItemContainer _itemContainer;
        private int _firstItemsValue;
        private int _additionalItemsValue;

        private void Awake()
        {
            _itemContainer = GetComponent<ItemContainer>();
        }

        private void OnEnable()
        {
            _itemContainer.ItemsActiveCountChanged += ItemsFirstListChangedValue;
        }

        private void OnDisable()
        {
            _itemContainer.ItemsActiveCountChanged -= ItemsFirstListChangedValue;
        }

        private void ItemsFirstListChangedValue(int value)
        {
            Debug.Log("value Items " + value);
            PlayerPrefs.SetInt("ItemContainer" + _itemContainer.CurrentItemContainer, value);
        }
    }
}