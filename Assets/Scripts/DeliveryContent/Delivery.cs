using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using SoContent;
using UI.Screens.ShopContent;
using UnityEngine;

namespace DeliveryContent
{
    public class Delivery : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private DeliveryConfig _deliveryConfig;

        private List<ItemDeliveryInfo> _items = new List<ItemDeliveryInfo>();
        private bool _isSpawning = false;
        private Coroutine _coroutine;
        private int _amountDeliveries;
        public event Action<int> AmountItemsDeliveriesChanged;

        private const string SavedItemsKey = "SavedDeliveryItems";
        private DateTime _lastExitTime;

        private void Start()
        {
            // Загружаем сохраненные данные при старте
            LoadDeliveryData();
            // Проверяем пропущенные доставки из-за отсутствия игрока

            // CheckMissedDeliveries();

            if (_items.Count > 0 && !_isSpawning)
            {
                SpawnItems();
            }
        }

        private void OnApplicationQuit()
        {
            // Сохраняем время выхода
            // SaveLastExitTime();
        }

        /*
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Debug.Log("pauseStatus " + pauseStatus);
                // Сохраняем время выхода при паузе (для мобильных устройств)
                SaveLastExitTime();
            }
            else
            {
                Debug.Log("pauseStatus " + pauseStatus);
                // Проверяем пропущенные доставки при возвращении
                CheckMissedDeliveries();
            }
        }
        */

        public void AddItemsCart(List<ItemCart> items)
        {
            foreach (var item in items)
            {
                _items.Add(new ItemDeliveryInfo(item.ItemType, item.CurrentAmount));
            }

            if (!_isSpawning)
            {
                SpawnItems();
            }

            UpdateAmountDeliveries();
            SaveDeliveryData();
        }

        private void SpawnItems()
        {
            _isSpawning = true;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (_items.Count > 0)
            {
                yield return new WaitForSeconds(_deliveryConfig.MinValueTimer);
                Debug.Log("СПАВН " + _items.Count);

                var item = _items[0];
                Debug.Log(" item.Amount " + item.Amount + "   " + item.ItemType);
                GameObject prefab = _deliveryConfig.GetPrefabByItemType(item.ItemType);

                if (prefab != null)
                {
                    Instantiate(prefab, _spawnPosition.position, Quaternion.identity);
                }

                item.Amount--;

                UpdateAmountDeliveries();

                if (item.Amount <= 0)
                {
                    _items.RemoveAt(0);
                }
                
                SaveDeliveryData();
            }

            _isSpawning = false;
        }

        private void UpdateAmountDeliveries()
        {
            _amountDeliveries = 0;

            foreach (var item in _items)
            {
                _amountDeliveries += item.Amount;
            }

            AmountItemsDeliveriesChanged?.Invoke(_amountDeliveries);

            Debug.Log($"Общее количество доставок: {_amountDeliveries}");
        }

        /*private void SaveLastExitTime()
        {
            PlayerPrefs.SetString(LastExitTimeKey, DateTime.UtcNow.ToString("O"));
            PlayerPrefs.Save();
        }*/

        private DeliverySaveWrapper ConvertToSaveData()
        {
            var wrapper = new DeliverySaveWrapper
            {
                SaveTime = DateTime.UtcNow.ToString("O")
            };

            foreach (var item in _items)
            {
                wrapper.Items.Add(new SavedItemData(item.ItemType, item.Amount));
            }

            return wrapper;
        }

        // 5. Конвертация после загрузки
        private List<ItemDeliveryInfo> ConvertFromSaveData(DeliverySaveWrapper wrapper)
        {
            var result = new List<ItemDeliveryInfo>();
            
            if (wrapper?.Items != null)
            {
                foreach (var savedItem in wrapper.Items)
                {
                    // Используем конструктор вместо приведения типов
                    result.Add(new ItemDeliveryInfo(
                        (ItemType)savedItem.ItemTypeInt,
                        savedItem.Amount
                    ));
                }
            }

            return result;
        }

        // 6. Сохранение данных
        private void SaveDeliveryData()
        {
            try
            {
                var saveData = ConvertToSaveData();
                string json = JsonUtility.ToJson(saveData);

                PlayerPrefs.SetString("DeliverySave_v4", json);
                PlayerPrefs.Save();

                Debug.Log($"Сохранено {_items.Count} предметов. JSON:\n{json}");
                LogItems(_items);
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка сохранения: {e.Message}");
            }
        }

        // 7. Загрузка данных
        private void LoadDeliveryData()
        {
            try
            {
                if (!PlayerPrefs.HasKey("DeliverySave_v4"))
                {
                    Debug.Log("Нет сохраненных данных");
                    _items = new List<ItemDeliveryInfo>();
                    return;
                }

                string json = PlayerPrefs.GetString("DeliverySave_v4");
                var saveData = JsonUtility.FromJson<DeliverySaveWrapper>(json);

                _items = ConvertFromSaveData(saveData);
                Debug.Log($"Загружено {_items.Count} предметов");
                LogItems(_items);
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка загрузки: {e.Message}");
                _items = new List<ItemDeliveryInfo>();
            }
        }

        private void LogItems(List<ItemDeliveryInfo> items)
        {
            foreach (var item in items)
            {
                Debug.Log($"{item.ItemType} ({(int)item.ItemType}) x{item.Amount}");
            }
        }


        [System.Serializable]
        private class ItemListWrapper
        {
            public List<ItemDeliveryInfo> Items;
        }
        
        [System.Serializable]
        private class DeliverySaveWrapper
        {
            public List<SavedItemData> Items = new List<SavedItemData>();
            public string SaveTime;
        }
        
        [System.Serializable]
        private class SavedItemData
        {
            public int ItemTypeInt;
            public int Amount;

            public SavedItemData(ItemType type, int amount)
            {
                ItemTypeInt = (int)type;
                Amount = amount;
            }
        }

/*private void SaveItems()
{
    string itemsJson = JsonUtility.ToJson(_items);
    PlayerPrefs.SetString("SavedDeliveryItems", itemsJson);
    PlayerPrefs.Save();
}

private void LoadSavedData()
{
    // Загружаем сохраненные предметы
    string savedItems = PlayerPrefs.GetString("SavedDeliveryItems", "");

    if (!string.IsNullOrEmpty(savedItems))
    {
        _items = JsonUtility.FromJson<List<ItemDeliveryInfo>>(savedItems);
    }

    // Загружаем время последнего выхода
    string lastExitTimeString = PlayerPrefs.GetString(LastExitTimeKey, "");

    if (!string.IsNullOrEmpty(lastExitTimeString))
    {
        _lastExitTime = DateTime.Parse(lastExitTimeString, null,
            System.Globalization.DateTimeStyles.RoundtripKind);
    }
}*/

/*private void CheckMissedDeliveries()
{
    if (!PlayerPrefs.HasKey(LastExitTimeKey)) return;

    DateTime currentTime = DateTime.UtcNow;
    TimeSpan absenceTime = currentTime - _lastExitTime;

    // Вычисляем сколько доставок пропущено
    float totalSeconds = (float)absenceTime.TotalSeconds;
    int missedDeliveries = Mathf.FloorToInt(totalSeconds / _deliveryConfig.MinValueTimer);

    Debug.Log($"Игрок отсутствовал {absenceTime.TotalMinutes} минут. Пропущено доставок: {missedDeliveries}");

    // Обрабатываем пропущенные доставки
    ProcessMissedDeliveries(missedDeliveries);

    // Обновляем количество
    UpdateAmountDeliveries();

    // Сохраняем изменения
    SaveItems();
}*/

/*private void ProcessMissedDeliveries(int missedCount)
{
    while (missedCount > 0 && _items.Count > 0)
    {
        ItemDeliveryInfo currentItem = _items[0];
        currentItem.Amount--;
        missedCount--;

        if (currentItem.Amount <= 0)
        {
            _items.RemoveAt(0);
            Debug.Log($"Автоматически завершена доставка: {currentItem.ItemType}");
        }
    }

    if (missedCount > 0)
    {
        Debug.Log($"Всего пропущено доставок: {missedCount} (включая уже завершённые)");
    }

    UpdateAmountDisplay();
    SaveGameData();
}*/
    }
}