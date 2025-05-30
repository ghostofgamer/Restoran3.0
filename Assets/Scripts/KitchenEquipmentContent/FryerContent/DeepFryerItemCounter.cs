using System;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class DeepFryerItemCounter : MonoBehaviour
    {
        private List<Item> _items = new List<Item>();
        
        public event Action<List<Item>> ItemsValueChanged;
        
        public void AddItem(Item item)
        {
            _items.Add(item);
            ItemsValueChanged?.Invoke(_items);
        }

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
            ItemsValueChanged?.Invoke(_items);
        }
    }
}