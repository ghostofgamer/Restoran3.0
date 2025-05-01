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

        private const string SavedItemsKey = "DeliverySaveData";
        private const string LastExitTimeKey = "LastExitTime";

        private void Start()
        {
            LoadDeliveryData();
            ProcessMissedDeliveries();
                
            if (_items.Count > 0 && !_isSpawning)
                SpawnItems();
        }

        private void OnApplicationQuit()
        {
            Debug.Log("сохраняем при выходе");
            SaveLastExitTime();
            SaveDeliveryData();
        }
        
        /*private void OnApplicationQuit()
        {
            SaveLastExitTime();
            SaveDeliveryData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveLastExitTime();
                SaveDeliveryData();
            }
            else
            {
                ProcessMissedDeliveries();
            }
        }*/
        
        private void ProcessMissedDeliveries()
{
    if (!PlayerPrefs.HasKey(LastExitTimeKey)) return;

    // Загружаем время выхода в UTC
    string savedTime = PlayerPrefs.GetString(LastExitTimeKey);
    DateTime lastExitTime;
    
    if (!DateTime.TryParse(savedTime, null, System.Globalization.DateTimeStyles.RoundtripKind, out lastExitTime))
    {
        Debug.LogError("Не удалось распарсить время выхода");
        return;
    }

    // Текущее время в UTC
    DateTime currentTime = DateTime.UtcNow;
    
    // Проверяем чтобы время не было в будущем (на случай проблем с системным временем)
    if (currentTime < lastExitTime)
    {
        Debug.LogWarning("Время выхода в будущем! Сброс времени.");
        lastExitTime = currentTime;
    }

    TimeSpan absenceTime = currentTime - lastExitTime;
    float totalSeconds = (float)absenceTime.TotalSeconds;
    
    // Минимальный интервал между доставками (защита от деления на 0)
    float spawnInterval = Mathf.Max(0.1f, _deliveryConfig.MinValueTimer);
    int missedDeliveries = Mathf.FloorToInt(totalSeconds / spawnInterval);

    Debug.Log($"Игрок отсутствовал {absenceTime.TotalSeconds} сек. Пропущено доставок: {missedDeliveries}");

    // Ограничиваем максимальное количество пропущенных доставок
    int maxMissedDeliveries = 100; // Например, не более 100 за раз
    missedDeliveries = Mathf.Min(missedDeliveries, maxMissedDeliveries);

    // Обрабатываем пропущенные доставки
    while (missedDeliveries > 0 && _items.Count > 0)
    {
        Debug.Log("Обработка пропущенной доставки");
        
        var item = _items[0];
        GameObject prefab = _deliveryConfig.GetPrefabByItemType(item.ItemType);
        
        if (prefab != null)
        {
            Instantiate(prefab, _spawnPosition.position, Quaternion.identity);
        }
        
        item.Amount--;
        missedDeliveries--;

        if (item.Amount <= 0)
        {
            _items.RemoveAt(0);
        }
    }

    UpdateAmountDeliveries();
    SaveDeliveryData();
}

private void SaveLastExitTime()
{
    // Сохраняем в формате ISO 8601 (UTC)
    PlayerPrefs.SetString(LastExitTimeKey, DateTime.UtcNow.ToString("O"));
    PlayerPrefs.Save();
    Debug.Log($"Время выхода сохранено: {DateTime.UtcNow.ToString("O")}");
}
        
        
        /*private void SaveLastExitTime()
        {
            PlayerPrefs.SetString(LastExitTimeKey, DateTime.UtcNow.ToString("O"));
            PlayerPrefs.Save();
        }
        
        private void ProcessMissedDeliveries()
        {
            if (!PlayerPrefs.HasKey(LastExitTimeKey)) return;

            DateTime lastExitTime;
            if (!DateTime.TryParse(PlayerPrefs.GetString(LastExitTimeKey), out lastExitTime))
                return;

            TimeSpan absenceTime = DateTime.UtcNow - lastExitTime;
            float totalSeconds = (float)absenceTime.TotalSeconds;
            int missedDeliveries = Mathf.FloorToInt(totalSeconds / _deliveryConfig.MinValueTimer);

            Debug.Log($"Игрок отсутствовал {absenceTime.TotalSeconds} сек. Пропущено доставок: {missedDeliveries}");

            // Обрабатываем пропущенные доставки
            while (missedDeliveries > 0 && _items.Count > 0)
            {
                Debug.Log(" Спавним ");
                
                var item = _items[0];
                
                GameObject prefab = _deliveryConfig.GetPrefabByItemType(item.ItemType);
                
                if (prefab != null)
                {
                    Instantiate(prefab, _spawnPosition.position, Quaternion.identity);
                }
                
                item.Amount--;
                missedDeliveries--;

                if (item.Amount <= 0)
                {
                    _items.RemoveAt(0);
                }
            }

            UpdateAmountDeliveries();
            SaveDeliveryData();
        }*/
        
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
        
        private void SaveDeliveryData()
        {
            try
            {
                var saveData = ConvertToSaveData();
                string json = JsonUtility.ToJson(saveData);

                PlayerPrefs.SetString(SavedItemsKey, json);
                PlayerPrefs.Save();

                Debug.Log($"Сохранено {_items.Count} предметов. JSON:\n{json}");
                LogItems(_items);
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка сохранения: {e.Message}");
            }
        }
        
        private void LoadDeliveryData()
        {
            try
            {
                if (!PlayerPrefs.HasKey(SavedItemsKey))
                {
                    Debug.Log("Нет сохраненных данных");
                    _items = new List<ItemDeliveryInfo>();
                    return;
                }

                string json = PlayerPrefs.GetString(SavedItemsKey);
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
    }
}