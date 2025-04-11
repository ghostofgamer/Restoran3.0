using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

namespace InteractableContent
{
    public class ItemContainer : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private Item[] _items;
        [SerializeField] private Transform[] _positions;

        [SerializeField] private Item[] _additionalItems;
        [SerializeField] private Transform[] _additioanlPositions;

        [SerializeField] private ItemType _currentItemContainer;
        [SerializeField] private AssemblyTable _assemblyTable;
        [SerializeField] private bool _isAdditionalItemsContainer;

        [SerializeField] private ItemType[] _currentItemsType;
        [SerializeField] private Item[][] _itemsAdditionalArray;

        public ItemType CurrentItemContainer => _currentItemContainer;

        public bool IsAdditionalItemsContainer => _isAdditionalItemsContainer;

        private void OnEnable()
        {
            _interactableObject.OnAction += _assemblyTable.HandlePlayerInteraction;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= _assemblyTable.HandlePlayerInteraction;
        }

        private void Start()
        {
            _itemsAdditionalArray = new Item[][] { _items, _additionalItems };
        }

        public void Click()
        {
            Debug.Log("НАЖИМАЮ НА КОНТЕЙНЕР");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("КОНТЕЙНЕР");
        }

        public int GetEmptyPosition()
        {
            int notActiveCount = 0;

            foreach (var item in _items)
            {
                if (item != null && !item.gameObject.activeSelf)
                    notActiveCount++;
            }

            Debug.Log("notActiveCount " + notActiveCount);
            return notActiveCount;
        }

        public int[] GetEmptyPositions()
        {
            int[] emptyPositions = new int[_itemsAdditionalArray.Length];

            for (int i = 0; i < _itemsAdditionalArray.Length; i++)
            {
                Item[] itemsToCheck = _itemsAdditionalArray[i];
                int emptyCount = 0;

                foreach (var item in itemsToCheck)
                {
                    if (item != null && !item.gameObject.activeSelf)
                    {
                        emptyCount++;
                    }
                }

                emptyPositions[i] = emptyCount;
            }

            return emptyPositions;
        }

        public void ActivateItems(int value)
        {
            if (_items == null)
            {
                Debug.LogError("_items array is not initialized.");
                return;
            }

            List<Item> inactiveItems = _items.Where(p => !p.gameObject.activeSelf).ToList();

            for (int i = 0; i < value; i++)
            {
                inactiveItems[i].gameObject.SetActive(true);
            }
        }

        public void ActivateItems(int value, int index)
        {
            if (_itemsAdditionalArray[index] == null)
            {
                Debug.LogError("_items array is not initialized.");
                return;
            }

            List<Item> inactiveItems = _itemsAdditionalArray[index].Where(p => !p.gameObject.activeSelf).ToList();

            for (int i = 0; i < value; i++)
            {
                inactiveItems[i].gameObject.SetActive(true);
            }
        }
    }
}