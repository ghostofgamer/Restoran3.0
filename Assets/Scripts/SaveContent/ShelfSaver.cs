using System;
using System.Collections.Generic;
using System.IO;
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
        [SerializeField] private bool _isBuyed;
        [SerializeField] private int _index;

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
            // LoadDataFromPlayerPrefs();


            List<ShelfItemData> loadedItems = LoadData();

            if (loadedItems.Count > 0)
            {
                // Восстановите предметы на шкафу
                _shelf.Initialization(loadedItems);
            }
        }

        /*private void SaveDate(List<ItemBasket> itemBasketList, List<ItemDrinkPackage> itemDrinkList)
        {
            int[] combinedIndices = itemBasketList.Select(item => (int)item.ItemType)
                .Concat(itemDrinkList.Select(item => (int)item.ItemType))
                .ToArray();

            string combinedIndicesString = string.Join(",", combinedIndices);

            if (!_isBuyed)
                PlayerPrefs.SetString("combinedItemIndices", combinedIndicesString);
            else
                PlayerPrefs.SetString("combinedItemIndices" + _index, combinedIndicesString);

            PlayerPrefs.Save();
        }

        private void LoadDataFromPlayerPrefs()
        {
            string combinedIndicesString;

            combinedIndicesString = !_isBuyed
                ? PlayerPrefs.GetString("combinedItemIndices", "")
                : PlayerPrefs.GetString("combinedItemIndices" + _index, "");

            if (!string.IsNullOrEmpty(combinedIndicesString))
            {
                string[] indicesArray = combinedIndicesString.Split(',');
                int[] indices = Array.ConvertAll(indicesArray, int.Parse);

                foreach (var index in indices)
                    _itemTypes.Add((ItemType)index);
            }

            if (_itemTypes.Count > 0)
                _shelf.Initialization(_itemTypes);
        }*/

        /*public async void SaveDate(List<ItemBasket> itemBasketList, List<ItemDrinkPackage> itemDrinkList)
        {
            try
            {
                // Преобразуем данные предметов в формат для сохранения
                List<ShelfItemData> itemsToSave = itemBasketList
                    .Select(item => new ShelfItemData(
                        (int)item.ItemType,
                        item.GetActiveValueItems(),
                        item.IsAdditionalItemsBasket,
                        item.GetActiveValueArrayItems().ToList()))
                    .Concat(itemDrinkList
                        .Select(item => new ShelfItemData(
                            (int)item.ItemType,
                            item.CurrentFullness,
                            false,
                            null)))
                    .ToList();

                // Сериализуем данные в JSON
                string jsonData = JsonUtility.ToJson(new ShelfItemDataWrapper(itemsToSave));

                // Путь к файлу
                string path = Path.Combine(Application.persistentDataPath, !_isBuyed ? "shelfData.json" : $"shelfData_{_index}.json");

                // Асинхронная запись в файл
                await File.WriteAllTextAsync(path, jsonData);
                Debug.Log($"Shelf data saved successfully to {path}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save shelf data: {ex.Message}");
            }
        }*/

        /*public async void SaveDate(List<ItemBasket> itemBasketList, List<ItemDrinkPackage> itemDrinkList)
        {
            try
            {
                if (itemBasketList == null)
                    itemBasketList = new List<ItemBasket>();
                if (itemDrinkList == null)
                    itemDrinkList = new List<ItemDrinkPackage>();


                // Преобразуем данные предметов в формат для сохранения
                List<ShelfItemData> itemsToSave = itemBasketList
                    .Where(item => item != null) // Игнорируем null-элементы
                    .Select(item => new ShelfItemData(
                        (int)item.ItemType,
                        item.GetActiveValueItems(),
                        item.IsAdditionalItemsBasket,
                        item.GetActiveValueArrayItems()?.ToList() ?? new List<int>()))
                    .Concat(itemDrinkList
                        .Where(item => item != null) // Игнорируем null-элементы
                        .Select(item => new ShelfItemData(
                            (int)item.ItemType,
                            item.CurrentFullness,
                            false,
                            null)))
                    .ToList();

                // Сериализуем данные в JSON
                string jsonData = JsonUtility.ToJson(new ShelfItemDataWrapper(itemsToSave));

                // Путь к файлу
                string path = Path.Combine(Application.persistentDataPath, !_isBuyed ? "shelfData.json" : $"shelfData_{_index}.json");

                // Убедимся, что директория существует
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                // Асинхронная запись в файл
                await File.WriteAllTextAsync(path, jsonData);
                Debug.Log($"Shelf data saved successfully to {path}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save shelf data: {ex.Message} {ex.StackTrace}");
            }
        }*/

        public async void SaveDate(List<ItemBasket> itemBasketList, List<ItemDrinkPackage> itemDrinkList)
        {
            try
            {
                if (itemBasketList == null)
                    itemBasketList = new List<ItemBasket>();
                if (itemDrinkList == null)
                    itemDrinkList = new List<ItemDrinkPackage>();

                // Преобразуем данные предметов в формат для сохранения
                List<ShelfItemData> itemsToSave = itemBasketList
                    .Where(item => item != null) // Игнорируем null-элементы
                    .Select(item =>
                    {
                        // Если корзина не является дополнительной, передаем null вместо вызова метода
                        List<int> additionalItems = item.IsAdditionalItemsBasket
                            ? item.GetActiveValueArrayItems()?.ToList() ?? new List<int>()
                            : null;

                        return new ShelfItemData(
                            (int)item.ItemType,
                            item.GetActiveValueItems(),
                            item.IsAdditionalItemsBasket,
                            additionalItems
                        );
                    })
                    .Concat(itemDrinkList
                        .Where(item => item != null) // Игнорируем null-элементы
                        .Select(item => new ShelfItemData(
                            (int)item.ItemType,
                            item.CurrentFullness,
                            false,
                            null)))
                    .ToList();

                // Сериализуем данные в JSON
                string jsonData = JsonUtility.ToJson(new ShelfItemDataWrapper(itemsToSave));

                // Путь к файлу
                string path = Path.Combine(Application.persistentDataPath,
                    !_isBuyed ? "shelfData.json" : $"shelfData_{_index}.json");

                // Убедимся, что директория существует
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                // Асинхронная запись в файл
                await File.WriteAllTextAsync(path, jsonData);
                Debug.Log($"Shelf data saved successfully to {path}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save shelf data: {ex.Message} {ex.StackTrace}");
            }
        }

        public List<ShelfItemData> LoadData()
        {
            string fileName = !_isBuyed ? "shelfData.json" : $"shelfData_{_index}.json";
            string path = Path.Combine(Application.persistentDataPath, fileName);

            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                ShelfItemDataWrapper wrapper = JsonUtility.FromJson<ShelfItemDataWrapper>(jsonData);
                return wrapper.items;
            }

            return new List<ShelfItemData>();
        }


        [System.Serializable]
        public struct ShelfItemData
        {
            public int itemType; // Тип предмета
            public int amount; // Количество предметов
            public bool isAdditional; // Флаг дополнительных предметов (если нужно)
            public List<int> additionalAmountItems; // Дополнительные предметы (если есть)

            public ShelfItemData(int type, int amount, bool isAdditional, List<int> addAmtItems)
            {
                itemType = type;
                this.amount = amount;
                this.isAdditional = isAdditional;
                additionalAmountItems = addAmtItems;
            }
        }

        [System.Serializable]
        public class ShelfItemDataWrapper
        {
            public List<ShelfItemData> items;

            public ShelfItemDataWrapper(List<ShelfItemData> items)
            {
                this.items = items;
            }
        }
    }
}