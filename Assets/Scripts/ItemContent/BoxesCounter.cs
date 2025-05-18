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
        private List<ItemDrinkPackage> _itemDrinkPackages = new List<ItemDrinkPackage>();

        public List<ItemBasket> ItemBaskets => _itemBaskets;
        public List<ItemDrinkPackage> ItemDrinkPackages => _itemDrinkPackages;

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
            Load();
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
                _itemDrinkPackages.Add(itemDrinkPackage);
            }
        }

        private void Load()
        {
            List<BoxData> loadedBoxes = _boxSaver.LoadData();
            Debug.Log("loadedBoxes " + loadedBoxes.Count);

            foreach (BoxData boxData in loadedBoxes)
            {
                GameObject prefab = _deliveryConfig.GetPrefabByItemType((ItemType)boxData.itemType);

                if (prefab != null)
                {
                    GameObject box = Instantiate(prefab, boxData.position, Quaternion.identity);
                    AddBox(box);
                }
            }
        }
    }
}