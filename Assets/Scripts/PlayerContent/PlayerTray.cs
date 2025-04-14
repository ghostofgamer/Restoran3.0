using System.Linq;
using UnityEngine;

namespace PlayerContent
{
    public class PlayerTray : MonoBehaviour
    {
        [SerializeField] private Item[] _rawCutlets;
        [SerializeField] private Item[] _readyCutlets;
        [SerializeField] private GameObject _rawCutletTray;
        [SerializeField] private GameObject _cutletTray;

        public ItemType CurrentType { get; private set; }

        public bool IsActive { get; private set; }

        public int GetEmptyPositionValue(ItemType itemType)
        {
            if (!IsActive)
            {
                Item[] itemsToCheck = GetItemsByType(itemType);

                if (itemsToCheck != null)
                {
                    int inactiveCount = itemsToCheck.Count(item => !item.gameObject.activeSelf);
                    Debug.Log("inactiveCount " + inactiveCount);
                    SetCurrentItemType(itemType);
                    return inactiveCount;
                }
            }

            return 0;
        }

        public int GetActivePositionValue(ItemType itemType)
        {
            if (IsActive)
            {
                Item[] itemsToCheck = GetItemsByType(itemType);

                if (itemsToCheck != null)
                {
                    int inactiveCount = itemsToCheck.Count(item => item.gameObject.activeSelf);
                    Debug.Log("activeCount " + inactiveCount);
                    SetCurrentItemType(itemType);
                    return inactiveCount;
                }
            }

            return 0;
        }

        public void Put(ItemType itemType, int value)
        {
            if (!IsActive)
            {
                IsActive = true;
                gameObject.SetActive(true);
                ActivateTray(itemType);
                ToggleItems(GetItemsByType(itemType), value, true);
            }
        }

        public void PutAway(ItemType itemType, int value)
        {
            if (IsActive)
            {
                ToggleItems(GetItemsByType(itemType), value, false);

                int activeItems = GetActivePositionValue(itemType);
                Debug.Log("Осталось " + activeItems);
                if (activeItems <= 0)
                {
                    DeactivateTray(itemType);
                    gameObject.SetActive(false);
                    IsActive = false;
                    CurrentType = ItemType.Empty;
                }
            }
        }

        private void ActivateTray(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.RawCutlet:
                    _rawCutletTray.SetActive(true);
                    break;
                case ItemType.Cutlet:
                    _cutletTray.SetActive(true);
                    break;
            }
        }

        private void DeactivateTray(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.RawCutlet:
                    _rawCutletTray.SetActive(false);
                    break;
                case ItemType.Cutlet:
                    _cutletTray.SetActive(false);
                    break;
            }
        }

        private void ToggleItems(Item[] items, int value, bool activate)
        {
            if (items != null)
            {
                int toggledCount = 0;

                foreach (var item in items)
                {
                    if (item.gameObject.activeSelf != activate)
                    {
                        item.gameObject.SetActive(activate);
                        toggledCount++;

                        if (toggledCount >= value)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void SetCurrentItemType(ItemType itemType)
        {
            CurrentType = itemType;
        }

        public void Clear()
        {
            IsActive = false;
        }

        private Item[] GetItemsByType(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.RawCutlet => _rawCutlets,
                ItemType.Cutlet => _readyCutlets,
                _ => null
            };
        }
    }
}