using System.Collections.Generic;
using InteractableContent;
using PlayerContent;
using UnityEngine;

public class AssemblyTable : MonoBehaviour
{
    [SerializeField] private ItemContainer[] _itemContainers;

    private Dictionary<ItemType, ItemContainer> _containersByItemType;

    private void Awake()
    {
        _containersByItemType = new Dictionary<ItemType, ItemContainer>();

        foreach (var container in _itemContainers)
        {
            _containersByItemType[container.CurrentItemContainer] = container;
        }
    }

    public void HandlePlayerInteraction(PlayerInteraction playerInteraction)
    {
        if (playerInteraction.CurrentDraggable != null)
        {
            ItemBasket basket = playerInteraction.CurrentDraggable.GetComponent<ItemBasket>();

            if (basket != null)
            {
                ItemContainer targetContainer = GetContainerForItemType(basket.ItemType);

                if (targetContainer != null)
                {
                    if (targetContainer.IsAdditionalItemsContainer && basket.IsAdditionalItemsBasket)
                    {
                        int[] emptyPositions = targetContainer.GetEmptyPositions();
                        int[] activeItems = basket.GetActiveValueArrayItems();

                        for (int i = 0; i < emptyPositions.Length; i++)
                        {
                            Debug.Log("emptyPositions " + emptyPositions[i]);
                        }

                        for (int i = 0; i < activeItems.Length; i++)
                        {
                            Debug.Log("activeItems " + activeItems[i]);
                        }

                        if (emptyPositions.Length == activeItems.Length)
                        {
                            Debug.Log("Одинаковое коолличество видов продуктов ");

                            for (int i = 0; i < emptyPositions.Length; i++)
                            {
                                if (emptyPositions[i] > 0 && activeItems[i] > 0)
                                {
                                    int itemsToPlace = Mathf.Min(emptyPositions[i], activeItems[i]);
                                    targetContainer.ActivateItems(itemsToPlace,i);
                                    basket.RemoveItem(itemsToPlace, i);
                                    Debug.Log("itemsToPlace " + itemsToPlace);
                                }
                                else
                                {
                                    Debug.Log(" ЛИБо нету места или активных продуктов");
                                }
                            }
                        }
                    }
                    else
                    {
                        int emptyPosition = targetContainer.GetEmptyPosition();
                        int activeItems = basket.GetActiveValueItems();

                        if (emptyPosition > 0 && activeItems > 0)
                        {
                            int itemsToPlace = Mathf.Min(emptyPosition, activeItems);
                            targetContainer.ActivateItems(itemsToPlace);
                            basket.RemoveItem(itemsToPlace);
                            Debug.Log($"Placed {itemsToPlace} items in container for {basket.ItemType}");
                        }
                        else
                        {
                            Debug.Log(
                                $"No space in container or no active items in basket. Container empty positions: {emptyPosition}, Basket active items: {activeItems}");
                        }
                    }
                }
                else
                {
                    Debug.LogError($"No container found for item type: {basket.ItemType}");
                }
            }
            else
            {
                Debug.LogError("The draggable object is not an ItemBasket.");
            }
        }
        else
        {
            Debug.Log("No draggable object in player's hands.");
        }
    }

    private ItemContainer GetContainerForItemType(ItemType itemType)
    {
        if (_containersByItemType.TryGetValue(itemType, out var container))
        {
            return container;
        }

        return null;
    }
}