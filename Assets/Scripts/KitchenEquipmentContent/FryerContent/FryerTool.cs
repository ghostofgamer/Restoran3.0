using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerTool : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private GameObject[] _rawItemObjects;
        [SerializeField] private Transform[] _positions;
        [SerializeField] private int _maxCount;

        public Transform[] Positions => _positions;
        
        public ItemType ItemType => _itemType;

        public bool IsFull { get; private set; }

        public int GetCountActiveItems()
        {
            int activeCount = 0;

            foreach (var item in _rawItemObjects)
            {
                if (item.activeInHierarchy)
                    activeCount++;
            }

            return activeCount;
        }

        public int GetCountInactiveItems()
        {
            int inactiveCount = 0;

            foreach (var item in _rawItemObjects)
            {
                if (!item.activeInHierarchy)
                    inactiveCount++;
            }

            return inactiveCount;
        }

        public void AllRawItemsDeactivate()
        {
            foreach (var rawItemObject in _rawItemObjects)
                rawItemObject.SetActive(false);
        }

        public void ActivateItems(int value)
        {
            if (_rawItemObjects.Length <= 0)
            {
                Debug.LogError("_items array is not initialized.");
                return;
            }

            List<GameObject> inactiveItems = _rawItemObjects.Where(p => !p.gameObject.activeSelf).ToList();

            for (int i = 0; i < value; i++)
                inactiveItems[i].gameObject.SetActive(true);
            
            /*List<GameObject> activeItems = itemObjects.Where(p => p.gameObject.activeSelf).ToList();
            if()*/

            /*List<Item> activeItems = _items.Where(p => p.gameObject.activeSelf).ToList();
            ItemsActiveCountChanged?.Invoke(activeItems.Count);*/
        }
    }
}