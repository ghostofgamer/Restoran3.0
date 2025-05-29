using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerContainer : MonoBehaviour
    {
        [SerializeField] private GameObject[] _items;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private Transform[] _positions;

        public GameObject[] Items => _items;

        public ItemType ItemType => _itemType;

        public int GetInactiveValue()
        {
            List<GameObject> inactiveItems = _items.Where(p => !p.gameObject.activeSelf).ToList();
            return inactiveItems.Count;
        }
        
        public void ActivateItems(int value)
        {
            if (_items.Length <= 0)
            {
                Debug.LogError("_items array is not initialized.");
                return;
            }

            List<GameObject> inactiveItems = _items.Where(p => !p.gameObject.activeSelf).ToList();

            for (int i = 0; i < value; i++)
                inactiveItems[i].gameObject.SetActive(true);

            /*List<Item> activeItems = _items.Where(p => p.gameObject.activeSelf).ToList();
            ItemsActiveCountChanged?.Invoke(activeItems.Count);*/
        }
    }
}