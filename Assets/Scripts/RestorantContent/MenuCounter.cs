using System.Collections.Generic;
using Enums;
using SoContent;
using UnityEngine;

namespace RestorantContent
{
    public class MenuCounter : MonoBehaviour
    {
        [SerializeField] private List<ItemType> _menuList;
        [SerializeField] private ItemsConfig _itemsConfig;
        
        private Dictionary<ItemType, List<ItemType>> _categoryDictionary;
        
        public List<ItemType> MenuList => _menuList;
        
        private void Awake()
        {
            Debug.Log("Awake");
            _itemsConfig.Initialize();
            _categoryDictionary = new Dictionary<ItemType, List<ItemType>>();
        }

        public void AddItem(ItemType itemType)
        {
            if (!_menuList.Contains(itemType))
                _menuList.Add(itemType);
            else
                Debug.Log($"{itemType} is already in the menu.");
        }

        public void RemoveItem(ItemType itemType)
        {
            if (_menuList.Contains(itemType))
                _menuList.Remove(itemType);
            else
                Debug.Log($"{itemType} is not in the menu.");
        }
        
        public void CategorizeMenuItems()
        {
            _categoryDictionary.Clear();
            
            foreach (var itemType in _menuList)
            {
                var itemConfig = _itemsConfig.GetItemConfig(itemType);
                if (itemConfig != null)
                {
                    if (!_categoryDictionary.ContainsKey(itemConfig.category))
                    {
                        _categoryDictionary[itemConfig.category] = new List<ItemType>();
                    }
                    
                    _categoryDictionary[itemConfig.category].Add(itemType);
                }
            }
            
            foreach (var category in _categoryDictionary)
            {
                Debug.Log($"Category: {category.Key}");
                foreach (var item in category.Value)
                {
                    Debug.Log($" - {item}");
                }
            }
        }
    }
}