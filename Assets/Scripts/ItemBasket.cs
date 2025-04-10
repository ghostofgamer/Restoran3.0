using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemBasket : MonoBehaviour
{
    [SerializeField] private ItemType _itemType;
    [SerializeField] private Item[] _items;

    public ItemType ItemType => _itemType;

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
}