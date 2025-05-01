using System;
using System.Collections;
using System.Collections.Generic;
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

        public void AddItemsCart(List<ItemCart> items)
        {
            foreach (var item in items)
            {
                _items.Add(new ItemDeliveryInfo { ItemType = item.ItemType, Amount = item.CurrentAmount });
            }

            if (!_isSpawning)
            {
                SpawnItems();
            }

            UpdateAmountDeliveries();
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
    }
}