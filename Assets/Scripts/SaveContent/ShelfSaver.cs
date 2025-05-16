using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using ItemContent;
using ShelfContent;
using UnityEngine;

namespace SaveContent
{
    public class ShelfSaver : MonoBehaviour
    {
        [SerializeField] private Shelf _shelf;

        private List<ItemType> _itemTypes = new List<ItemType>();

        private void OnEnable()
        {
            _shelf.ListItemChanged += SaveDate;
        }

        private void OnDisable()
        {
            _shelf.ListItemChanged -= SaveDate;
        }

        private void Start()
        {
            LoadDataFromPlayerPrefs();
        }

        private void SaveDate(List<ItemBasket> itemBasketList, List<ItemDrinkPackage> itemDrinkList)
        {
            int[] combinedIndices = itemBasketList.Select(item => (int)item.ItemType)
                .Concat(itemDrinkList.Select(item => (int)item.ItemType))
                .ToArray();

            string combinedIndicesString = string.Join(",", combinedIndices);

            PlayerPrefs.SetString("combinedItemIndices", combinedIndicesString);
            PlayerPrefs.Save();
        }

        private void LoadDataFromPlayerPrefs()
        {
            string combinedIndicesString = PlayerPrefs.GetString("combinedItemIndices", "");

            if (!string.IsNullOrEmpty(combinedIndicesString))
            {
                string[] indicesArray = combinedIndicesString.Split(',');
                int[] indices = Array.ConvertAll(indicesArray, int.Parse);

                foreach (var index in indices)
                    _itemTypes.Add((ItemType)index);
            }
            
            if(_itemTypes.Count>0)
                _shelf.Initialization(_itemTypes);
        }
    }
}