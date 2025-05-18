using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using ItemContent;
using ShelfContent;
using UnityEngine;

public class ItemBasket : MonoBehaviour
{
    [SerializeField] private Draggable _draggable;
    [SerializeField] private ItemType _itemType;
    [SerializeField] private Item[] _items;
    [SerializeField] private ActivityItem[] _itemsActivity;

    [SerializeField] private Item[] _additionalItems;
    [SerializeField] private ActivityItem[] _additionalItemsActivity;

    [SerializeField] private bool _isAdditionalItemsBasket;
    [SerializeField] private ItemType[] _currentItemsType;

    [SerializeField] private Transform[] _positions;

    public Shelf Shelf { get; private set; }
    
    public Transform[] Positions => _positions;

    public ItemType ItemType => _itemType;

    public bool IsAdditionalItemsBasket => _isAdditionalItemsBasket;

    public ItemType[] CurrentItemsTypes => _currentItemsType;

    [SerializeField] private Item[][] _itemsAdditionalArray;
    [SerializeField] private ActivityItem[][] _itemsActivityAdditionalArray;

    private void OnEnable()
    {
        _draggable.DraggablePicked += ActivateItems;
        _draggable.DraggableThrowed += DeactivateItems;
        _draggable.PutOnShelfCompleting += DeactivateItems;
    }

    private void OnDisable()
    {
        _draggable.DraggablePicked -= ActivateItems;
        _draggable.DraggableThrowed -= DeactivateItems;
        _draggable.PutOnShelfCompleting -= DeactivateItems;
    }

    private void Start()
    {
        _itemsAdditionalArray = new Item[][] { _items, _additionalItems };
        _itemsActivityAdditionalArray = new ActivityItem[][] { _itemsActivity, _additionalItemsActivity };
        DeactivateItems();
    }

    public int GetActiveValueItems()
    {
        int activeCount = 0;

        /*foreach (var item in _items)
        {
            if (item != null && item.gameObject.activeSelf)
                activeCount++;
        }*/

        foreach (var item in _itemsActivity)
        {
            if (item != null && item.IsActive)
                activeCount++;
        }

        Debug.Log("ActiveCount " + activeCount);
        return activeCount;
    }

    public int[] GetActiveValueArrayItems()
    {
        int[] activeCounts = new int[_itemsActivityAdditionalArray.Length];

        for (int i = 0; i < _itemsActivityAdditionalArray.Length; i++)
        {
            int rowActiveCount = 0;
            var itemsRow = _itemsActivityAdditionalArray[i];

            foreach (var item in itemsRow)
            {
                if (item != null && item.IsActive)
                    rowActiveCount++;
            }

            activeCounts[i] = rowActiveCount;
            Debug.Log("ActiveCount in row " + i + ": " + rowActiveCount);
        }

        Debug.Log("Total ActiveCounts: " + string.Join(", ", activeCounts));
        return activeCounts;

        /*int[] activeCounts = new int[_itemsAdditionalArray.Length];

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
        return activeCounts;*/
    }

    public void RemoveItem(int value)
    {
        if (_items == null)
        {
            Debug.LogError("_items array is not initialized.");
            return;
        }

        // List<Item> inactiveItems = _items.Where(p => p.gameObject.activeSelf).ToList();
        List<ActivityItem> inactiveItems = _itemsActivity.Where(p => p.IsActive).ToList();

        if (value > inactiveItems.Count)
            value = inactiveItems.Count;

        /*for (int i = 0; i < value; i++)
            inactiveItems[i].gameObject.SetActive(false);*/

        for (int i = inactiveItems.Count - 1; i >= inactiveItems.Count - value; i--)
        {
            Debug.Log("ААА " + i);
            inactiveItems[i].SetValue(false);
            // inactiveItems[i].gameObject.SetActive(false);
        }
    }

    public void TransferProduct(int value, Transform[] positions)
    {
        if (_items == null)
        {
            Debug.LogError("_items array is not initialized.");
            return;
        }

        List<ActivityItem> inactiveItems = _itemsActivity.Where(p => p.IsActive).ToList();

        if (value > inactiveItems.Count)
            value = inactiveItems.Count;

        for (int i = inactiveItems.Count - 1; i >= inactiveItems.Count - value; i--)
        {
            Debug.Log("ААА " + i);

            int index = i; // Создаем локальную переменную для индекса

            inactiveItems[index].transform.DOMove(positions[index].transform.position, 0.15f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    inactiveItems[index].transform.localPosition = Vector3.zero;
                    inactiveItems[index].SetValue(false);
                });
        }
    }

    public void TransferProduct(int value, int index, Transform[][] positions)
    {
        if (_itemsActivityAdditionalArray == null || _itemsActivityAdditionalArray[index] == null)
        {
            Debug.LogError("_itemsActivityAdditionalArray array is not initialized.");
            return;
        }

        List<ActivityItem> inactiveItems = _itemsActivityAdditionalArray[index].Where(p => p.IsActive).ToList();

        if (value > inactiveItems.Count)
            value = inactiveItems.Count;

        for (int i = inactiveItems.Count - 1; i >= inactiveItems.Count - value; i--)
        {
            Debug.Log("ААА " + i);

            int itemIndex = i; // Создаем локальную переменную для индекса

            inactiveItems[itemIndex].transform.DOMove(positions[index][itemIndex].transform.position, 0.15f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    inactiveItems[itemIndex].transform.localPosition = Vector3.zero;
                    inactiveItems[itemIndex].SetValue(false);
                });
        }
    }

    public void RemoveItem(int value, int index)
    {
        if (_itemsActivityAdditionalArray[index] == null)
        {
            Debug.LogError("_itemsActivityAdditionalArray array is not initialized.");
            return;
        }

        List<ActivityItem> inactiveItems = _itemsActivityAdditionalArray[index].Where(p => p.IsActive).ToList();

        if (value > inactiveItems.Count)
            value = inactiveItems.Count;

        for (int i = inactiveItems.Count - 1; i >= inactiveItems.Count - value; i--)
        {
            Debug.Log("ААА " + i);
            inactiveItems[i].SetValue(false);
        }

        /*if (_itemsAdditionalArray[index] == null)
        {
            Debug.LogError("_items array is not initialized.");
            return;
        }

        List<Item> inactiveItems = _itemsAdditionalArray[index].Where(p => p.gameObject.activeSelf).ToList();

        if (value > inactiveItems.Count)
            value = inactiveItems.Count;

        /*for (int i = 0; i < value; i++)
            inactiveItems[i].gameObject.SetActive(false);#1#

        for (int i = inactiveItems.Count - 1; i >= inactiveItems.Count - value; i--)
        {
            Debug.Log("ААА " + i);
            inactiveItems[i].gameObject.SetActive(false);
        }*/
    }

    public void SetActiveValue(bool value)
    {
        foreach (var item in _itemsActivity)
            if (item != null & item.IsActive)
                item.gameObject.SetActive(value);

        foreach (var item in _additionalItemsActivity)
            if (item != null & item.IsActive)
                item.gameObject.SetActive(value);
    }

    private void ActivateItems()
    {
        SetActiveValue(true);
        
        if (Shelf != null)
        {
            Shelf.Remove(this);
            Shelf = null;
        }
    }

    private void DeactivateItems()
    {
        SetActiveValue(false);
    }

    public void SetShelf(Shelf shelf)
    {
        Shelf = shelf;
    }
}