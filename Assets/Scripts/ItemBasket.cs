using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemBasket : MonoBehaviour
{
    [SerializeField] private ItemType _itemType;
    [SerializeField] private Item[] _items;
    [SerializeField] private Item[] _additionalItems;
    [SerializeField] private bool _isAdditionalItemsBasket;
    
    [SerializeField] private ItemType[] _currentItemsType;
    
    public ItemType ItemType => _itemType;
    
    public bool IsAdditionalItemsBasket => _isAdditionalItemsBasket;
    
    [SerializeField]private Item[][] _itemsAdditionalArray;

    private void Start()
    {
        _itemsAdditionalArray = new Item[][] { _items, _additionalItems };
    }

    public int GetActiveValueItems()
    {
        int activeCount = 0;

        foreach (var item in _items)
        {
            if (item != null && item.gameObject.activeSelf)
                activeCount++;
        }

        Debug.Log("ActiveCount " + activeCount);
        return activeCount;
    }
    
    public int[] GetActiveValueArrayItems()
    {
        int[] activeCounts = new int[_itemsAdditionalArray.Length];

        for (int i = 0; i < _itemsAdditionalArray.Length; i++)
        {
            int rowActiveCount = 0;
            var itemsRow = _itemsAdditionalArray[i];

            foreach (var item in itemsRow)
            {
                if (item != null && item.gameObject.activeSelf)
                {
                    rowActiveCount++;
                }
            }

            activeCounts[i] = rowActiveCount;
            Debug.Log("ActiveCount in row " + i + ": " + rowActiveCount);
        }

        Debug.Log("Total ActiveCounts: " + string.Join(", ", activeCounts));
        return activeCounts;
    }

    public void RemoveItem(int value)
    {
        if (_items == null)
        {
            Debug.LogError("_items array is not initialized.");
            return;
        }

        List<Item> inactiveItems = _items.Where(p => p.gameObject.activeSelf).ToList();

        for (int i = 0; i < value; i++)
            inactiveItems[i].gameObject.SetActive(false);
    }
    
    public void RemoveItem(int value,int index)
    {
        if (_itemsAdditionalArray[index] == null)
        {
            Debug.LogError("_items array is not initialized.");
            return;
        }

        List<Item> inactiveItems = _itemsAdditionalArray[index].Where(p => p.gameObject.activeSelf).ToList();

        for (int i = 0; i < value; i++)
            inactiveItems[i].gameObject.SetActive(false);
    }
}