using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private DeliveryViewer _deliveryViewer;

        private List<ItemDeliveryInfo> _items = new List<ItemDeliveryInfo>();
        private bool _isSpawning = false;
        private Coroutine _coroutine;
        private int _amountDeliveries;
        private float _remainingTimeForNextSpawn;

        public event Action<int> AmountItemsDeliveriesChanged;

        public event Action<float> DeliveryTimerStarted;
        public event Action DeliveryTimerStopped;

        private const string SavedItemsKey = "DeliverySaveData";
        private const string LastExitTimeKey = "LastExitTime";
        private const string RemainingTimeKey = "RemainingTimeKey";

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
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Debug.Log("Приложение развернуто, обновляем данные");
                LoadDeliveryData();
                ProcessMissedDeliveries();
                if (_items.Count > 0 && !_isSpawning)
                    SpawnItems();
            }
            else
            {
                Debug.Log("Приложение свернуто, сохраняем данные");
                SaveLastExitTime();
                // SaveLastFocusTime();
                SaveDeliveryData();
            }
        }

        private void ProcessMissedDeliveries()
        {
            if (!PlayerPrefs.HasKey(LastExitTimeKey)) return;

            // Загружаем время выхода
            string savedTime = PlayerPrefs.GetString(LastExitTimeKey);
            DateTime lastExitTime;

            if (!DateTime.TryParse(savedTime, null, System.Globalization.DateTimeStyles.RoundtripKind,
                    out lastExitTime))
            {
                Debug.LogError("Не удалось распарсить время выхода");
                return;
            }

            // Загружаем оставшееся время на момент выхода
            float remainingTimeOnExit = PlayerPrefs.GetFloat(RemainingTimeKey, _deliveryConfig.MinValueTimer);

            // Текущее время
            DateTime currentTime = DateTime.UtcNow;

            Debug.Log($"lastExitTime: {lastExitTime}, currentTime: {currentTime}");
            Debug.Log($"remainingTimeOnExit: {remainingTimeOnExit}");

            // Проверка на некорректное время
            if (currentTime < lastExitTime)
            {
                Debug.LogWarning("Время выхода в будущем! Сброс времени.");
                lastExitTime = currentTime;
            }

            TimeSpan absenceTime = currentTime - lastExitTime;
            float totalSeconds = (float)absenceTime.TotalSeconds;
            float spawnInterval = _deliveryConfig.MinValueTimer;

            Debug.Log($"absenceTime: {absenceTime}");
            Debug.Log($"totalSeconds: {totalSeconds}");
            Debug.Log($"spawnInterval: {spawnInterval}");

            if (totalSeconds < remainingTimeOnExit)
            {
                _remainingTimeForNextSpawn = remainingTimeOnExit - totalSeconds;
                Debug.Log("ИИИ " + _remainingTimeForNextSpawn);
            }
            else
            {
                Debug.Log("УДЫУ");
                int fullDeliveries = 0;

                totalSeconds -= remainingTimeOnExit;
                fullDeliveries = 1;

                Debug.Log($"NEW TOTSL SEC : {totalSeconds}");
                // Общее время, которое нужно обработать (оставшееся + время отсутствия)
                // float totalProcessingTime = remainingTimeOnExit + totalSeconds;

                // Количество полных доставок за это время
                int deliversCount = Mathf.FloorToInt(totalSeconds / spawnInterval);

                fullDeliveries += deliversCount;

                // int fullDeliveries = Mathf.FloorToInt(totalProcessingTime / remainingTimeOnExit);

                // Новое оставшееся время для следующей доставки
                _remainingTimeForNextSpawn = spawnInterval - (totalSeconds % spawnInterval);

                Debug.Log($"_remainingTimeForNextSpawn: {_remainingTimeForNextSpawn}");
                Debug.Log($"_remainingTimeForNextSpawn: {fullDeliveries}");

                /*Debug.Log(
                    $"totalProcessingTime: {totalProcessingTime}, fullDeliveries: {fullDeliveries}, newRemainingTime: {_remainingTimeForNextSpawn}");*/

                // Ограничиваем максимальное количество пропущенных доставок
                fullDeliveries = Mathf.Min(fullDeliveries, 100);

                // Обрабатываем пропущенные доставки
                while (fullDeliveries > 0 && _items.Count > 0)
                {
                    var item = _items[0];
                    GameObject prefab = _deliveryConfig.GetPrefabByItemType(item.ItemType);

                    if (prefab != null)
                    {
                        Instantiate(prefab, _spawnPosition.position, Quaternion.identity);
                    }

                    item.Amount--;
                    fullDeliveries--;

                    if (item.Amount <= 0)
                    {
                        _items.RemoveAt(0);
                    }
                }

                if (_items.Count <= 0)
                    _remainingTimeForNextSpawn = 0;
            }

            UpdateAmountDeliveries();
            SaveDeliveryData();
        }

        private void SaveLastExitTime()
        {
            PlayerPrefs.SetString(LastExitTimeKey, DateTime.UtcNow.ToString("O"));
            PlayerPrefs.SetFloat(RemainingTimeKey, _deliveryViewer.CurrentTimer);
            PlayerPrefs.Save();
            Debug.Log($"Сохранено время выхода и оставшееся время: {_deliveryViewer.CurrentTimer}");
        }

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
            if (_remainingTimeForNextSpawn > 0 && _items.Count > 0)
            {
                Debug.Log($"SpawnFirstItem: " + _remainingTimeForNextSpawn);

                DeliveryTimerStarted?.Invoke(_remainingTimeForNextSpawn);
                yield return new WaitForSeconds(_remainingTimeForNextSpawn);

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

                _remainingTimeForNextSpawn = 0;
            }
            else if (_items.Count <= 0)
            {
                _remainingTimeForNextSpawn = 0;
            }

            // DeliveryTimerStarted?.Invoke(_deliveryConfig.MinValueTimer);

            while (_items.Count > 0)
            {
                DeliveryTimerStarted?.Invoke(_deliveryConfig.MinValueTimer);
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

            DeliveryTimerStopped?.Invoke();
            _remainingTimeForNextSpawn = 0;
            _isSpawning = false;
            SaveDeliveryData();
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

        private List<ItemDeliveryInfo> ConvertFromSaveData(DeliverySaveData saveData)
        {
            var result = new List<ItemDeliveryInfo>();

            if (saveData?.Items != null)
            {
                foreach (var savedItem in saveData.Items)
                {
                    result.Add(new ItemDeliveryInfo(
                        (ItemType)savedItem.ItemTypeInt,
                        savedItem.Amount
                    ));
                }
            }

            return result;
        }

        [System.Serializable]
        private class DeliverySaveData
        {
            public List<SavedItemData> Items = new List<SavedItemData>();
            public float RemainingTime;
            public string SaveTime;
        }

        private void SaveDeliveryData()
        {
            try
            {
                var saveData = new DeliverySaveData
                {
                    Items = _items.Select(i => new SavedItemData(i.ItemType, i.Amount)).ToList(),
                    RemainingTime = _remainingTimeForNextSpawn,
                    SaveTime = DateTime.UtcNow.ToString("O")
                };

                string json = JsonUtility.ToJson(saveData);
                PlayerPrefs.SetString(SavedItemsKey, json);
                PlayerPrefs.Save();
            }
            catch
            {
                /* обработка ошибок */
            }
        }

        private void LoadDeliveryData()
        {
            try
            {
                if (!PlayerPrefs.HasKey(SavedItemsKey))
                {
                    _items = new List<ItemDeliveryInfo>();
                    _remainingTimeForNextSpawn = _deliveryConfig.MinValueTimer;
                    return;
                }

                string json = PlayerPrefs.GetString(SavedItemsKey);
                var saveData = JsonUtility.FromJson<DeliverySaveData>(json);

                _items = ConvertFromSaveData(saveData);
                _remainingTimeForNextSpawn = saveData.RemainingTime;
            }
            catch
            {
                _items = new List<ItemDeliveryInfo>();
                _remainingTimeForNextSpawn = _deliveryConfig.MinValueTimer;
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