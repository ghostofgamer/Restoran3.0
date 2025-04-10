using System.Collections.Generic;
using PlayerContent;
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

        private void OnEnable()
        {
            _interactableObject.OnAction += LayProducts;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= LayProducts;
        }

        public virtual void LayProducts(PlayerInteraction playerInteraction)
        {
            if (playerInteraction.CurrentDraggable != null)
            {
                ItemBasket basket = playerInteraction.CurrentDraggable.GetComponent<ItemBasket>();

                if (basket != null)
                {
                    int emptyPosition = GetEmptyPosition();
                    int activeItems = basket.GetActiveValueItems();

                    if (emptyPosition > 0 && activeItems > 0)
                    {
                        int itemsToPlace = Mathf.Min(emptyPosition, activeItems);
                        ActivateItems(itemsToPlace);
                        basket.RemoveItem(itemsToPlace);
                        Debug.Log($"Mathf.Min {itemsToPlace} ");
                    }
                    else
                    {
                        Debug.Log($"В контейнере свободно {emptyPosition} , в корзине {activeItems}");
                    }
                }
                else
                {
                    Debug.Log("Непойми что в руках");
                }
            }

            if (playerInteraction.CurrentDraggable != null)
            {
                Debug.Log("Return");
            }
            else
            {
                Debug.Log("DRAG");
            }
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

        public void ActivateItems(int value)
        {
            if (_items == null)
            {
                Debug.LogError("_items array is not initialized.");
                return;
            }
            
            List<Item> inactiveItems = _items.Where(p=> !p.gameObject.activeSelf).ToList();

            for (int i = 0; i < value; i++)
            {
                inactiveItems[i].gameObject.SetActive(true);
            }
        }
    }
}