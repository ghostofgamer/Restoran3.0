using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using KitchenEquipmentContent.AssemblyTables.SodaTableContent;
using UnityEngine;

namespace SaveContent
{
    public class SodaSaver : MonoBehaviour
    {
        [SerializeField] private SodaFullnessCounter[] _sodaFullnessCounters;
        [SerializeField] private SodaCounter _sodaCounter;

        private void OnEnable()
        {
            foreach (var sodaFullnessCounter in _sodaFullnessCounters)
                sodaFullnessCounter.FullnessSodaChanged += SaveFullnessSoda;

            _sodaCounter.SodaItemsValueChanged += SaveWellSoda;
        }

        private void OnDisable()
        {
            foreach (var sodaFullnessCounter in _sodaFullnessCounters)
                sodaFullnessCounter.FullnessSodaChanged -= SaveFullnessSoda;

            _sodaCounter.SodaItemsValueChanged -= SaveWellSoda;
        }

        private void SaveFullnessSoda(ItemType itemType, int value)
        {
            PlayerPrefs.SetInt("SodaFullness" + itemType, value);
        }

        private void SaveWellSoda(List<Item> items)
        {
            int[] itemTypeIndices = items.Select(item => (int)item.ItemType).ToArray();

            // Сохраняем массив индексов ItemType в PlayerPrefs
            string indicesString = string.Join(",", itemTypeIndices);
            PlayerPrefs.SetString("SodaItemTypeIndices", indicesString);
            PlayerPrefs.Save();
        }

        public List<ItemType> LoadItemTypesFromIndices()
        {
            // Загружаем строку индексов ItemType из PlayerPrefs
            string indicesString = PlayerPrefs.GetString("SodaItemTypeIndices", "");

            // Преобразуем строку индексов в массив int
            int[] itemTypeIndices = indicesString.Split(',')
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(int.Parse)
                .ToArray();

            // Преобразуем массив индексов в список ItemType
            List<ItemType> itemTypes = itemTypeIndices.Select(index => (ItemType)index).ToList();

            return itemTypes;
        }
    }
}