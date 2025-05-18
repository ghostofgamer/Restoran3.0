using System;
using System.Collections.Generic;
using DeliveryContent;
using Enums;
using SaveContent;
using SoContent;
using UnityEngine;

namespace ItemContent
{
    public class BoxesCounter : MonoBehaviour
    {
        [SerializeField] private Delivery _delivery;
        [SerializeField] private DeliveryConfig _deliveryConfig;
        [SerializeField] private BoxSaver _boxSaver;

        private List<ItemBasket> _itemBaskets = new List<ItemBasket>();

        private List<BoxData> _itemType = new List<BoxData>();
        public List<ItemBasket> ItemBaskets => _itemBaskets;

        private void OnEnable()
        {
            _delivery.SpawnCompleted += AddBox;
        }

        private void OnDisable()
        {
            _delivery.SpawnCompleted -= AddBox;
        }

        private void Start()
        {
            // _itemType = _boxSaver.LoadData();

            List<BoxData> loadedBoxes = _boxSaver.LoadData();
            Debug.Log("loadedBoxes " + loadedBoxes.Count);
            
            foreach (BoxData boxData in loadedBoxes)
            {
                GameObject prefab = _deliveryConfig.GetPrefabByItemType((ItemType)boxData.itemType);
                
                if (prefab != null)
                    Instantiate(prefab, boxData.position, Quaternion.identity);
                
                /*GameObject box = Instantiate(boxPrefab, boxData.position, Quaternion.identity);
                // Дополнительная логика для настройки коробки*/
            }
            
            
            
            
            
            /*if (_itemType.Count > 0)
            {
                foreach (var itemType in _itemType)
                {
                    GameObject prefab = _deliveryConfig.GetPrefabByItemType(itemType);
                    
                    if (prefab != null)
                        Instantiate(prefab, _spawnPosition.position, Quaternion.identity);
                }
            }*/
        }

        private void AddBox(GameObject box)
        {
            if (box.TryGetComponent(out ItemBasket itemBasket))
            {
                Debug.Log("!Добавили баскет бокс");
                _itemBaskets.Add(itemBasket);
            }

            if (box.TryGetComponent(out ItemDrinkPackage itemDrinkPackage))
            {
            }
        }
    }
}