using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace RestorantContent
{
    public class MenuCounter : MonoBehaviour
    {
        [SerializeField] private List<ItemType> _menuList;

        public void AddItem(ItemType itemType)
        {
            if (!_menuList.Contains(itemType))
            {
                _menuList.Add(itemType);
                Debug.Log($"{itemType} added to the menu.");
            }
            else
            {
                Debug.Log($"{itemType} is already in the menu.");
            }
        }

        public void RemoveItem(ItemType itemType)
        {
            if (_menuList.Contains(itemType))
            {
                _menuList.Remove(itemType);
                Debug.Log($"{itemType} removed from the menu.");
            }
            else
            {
                Debug.Log($"{itemType} is not in the menu.");
            }
        }
    }
}