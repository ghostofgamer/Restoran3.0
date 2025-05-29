using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerTool : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private GameObject[] _rawItemObjects;
        [SerializeField] private GameObject[] _wellItemObjects;
        [SerializeField] private Transform[] _positions;
        [SerializeField] private int _maxCount;
        [SerializeField] private FryerToolMover _fryerToolMover;

        public Transform[] Positions => _positions;

        public ItemType ItemType => _itemType;

        public bool IsFull { get; private set; }

        public bool IsRaw { get; private set; } = true;

        public int GetCountActiveItems()
        {
            List<GameObject> activeItems = _rawItemObjects.Where(p => p.gameObject.activeSelf).ToList();
            return activeItems.Count;
        }

        public int GetCountActiveWellItems()
        {
            List<GameObject> activeItems = _wellItemObjects.Where(p => p.gameObject.activeSelf).ToList();
            return activeItems.Count;
        }

        public int GetCountInactiveItems()
        {
            List<GameObject> inactiveItems = _rawItemObjects.Where(p => !p.gameObject.activeSelf).ToList();
            return inactiveItems.Count;
        }

        public void AllRawItemsDeactivate()
        {
            foreach (var rawItemObject in _rawItemObjects)
                rawItemObject.SetActive(false);
        }

        public void AllWellItemsDeactivate()
        {
            foreach (var rawItemObject in _wellItemObjects)
                rawItemObject.SetActive(false);
        }

        public void ActivateRawItems(int value)
        {
            if (_rawItemObjects.Length <= 0)
            {
                Debug.LogError("_items array is not initialized.");
                return;
            }

            List<GameObject> inactiveItems = _rawItemObjects.Where(p => !p.gameObject.activeSelf).ToList();

            for (int i = 0; i < value; i++)
                inactiveItems[i].gameObject.SetActive(true);

            /*List<GameObject> activeItems = itemObjects.Where(p => p.gameObject.activeSelf).ToList();
            if()*/

            /*List<Item> activeItems = _items.Where(p => p.gameObject.activeSelf).ToList();
            ItemsActiveCountChanged?.Invoke(activeItems.Count);*/
        }

        public void ActivateWellItems()
        {
            int value = _rawItemObjects.Count(p => p.gameObject.activeSelf);
            AllRawItemsDeactivate();
            AllWellItemsDeactivate();

            for (int i = 0; i < value; i++)
                _wellItemObjects[i].SetActive(true);

            IsRaw = false;
        }

        public void DeactivateAllWellItems()
        {
            foreach (var wellItemObject in _wellItemObjects)
                wellItemObject.SetActive(false);

            IsRaw = true;
        }

        public void DeactivateWellItems(int value)
        {
            List<GameObject> activeItems = _wellItemObjects.Where(p => p.gameObject.activeSelf).ToList();

            for (int i = 0; i < value; i++)
                activeItems[i].gameObject.SetActive(false);

            List<GameObject> active = _wellItemObjects.Where(p => p.gameObject.activeSelf).ToList();
            
            if (active.Count <= 0)
                IsRaw = true;
        }

        public void MoveFrying()
        {
            _fryerToolMover.MoveFrying();
        }
    }
}