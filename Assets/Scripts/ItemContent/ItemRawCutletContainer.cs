using InteractableContent;
using PlayerContent;
using UnityEngine;

namespace ItemContent
{
    public class ItemRawCutletContainer : ItemContainer
    {
        public override void ActionContainer(PlayerInteraction playerInteraction)
        {
            if (playerInteraction.CurrentDraggable != null)
            {
                ItemBasket basket = playerInteraction.CurrentDraggable.GetComponent<ItemBasket>();

                if (basket != null && basket.ItemType == ItemType.RawCutlet)
                {
                    int emptyPosition = GetEmptyPosition();
                    int activeItems = basket.GetActiveValueItems();

                    if (emptyPosition > 0 && activeItems > 0)
                    {
                        int itemsToPlace = Mathf.Min(emptyPosition, activeItems);
                        basket.TransferProduct(itemsToPlace, Positions);
                        ActivateItems(itemsToPlace);
                        Debug.Log($"Placed {itemsToPlace} items in container for {basket.ItemType}");
                    }
                    else
                    {
                        Debug.Log(
                            $"No space in container or no active items in basket. Container empty positions: {emptyPosition}, Basket active items: {activeItems}");
                    }
                }
                else
                {
                    Debug.Log($"basket  NULL");
                }
            }
            else if (playerInteraction.PlayerTray)
            {
                if (playerInteraction.PlayerTray.CurrentType == ItemType.RawCutlet ||
                    playerInteraction.PlayerTray.CurrentType == ItemType.Empty)
                {
                    if (!playerInteraction.PlayerTray.IsActive)
                    {
                        int activeItems = GetActiveItemsValue();
                        int emptyPos = playerInteraction.PlayerTray.GetEmptyPositionValue(CurrentItemContainer);
                        int itemsToPlace = Mathf.Min(activeItems, emptyPos);

                        if (itemsToPlace > 0)
                        {
                            DeactivateItems(itemsToPlace);
                            playerInteraction.PlayerTray.Put(CurrentItemContainer, itemsToPlace);
                            Debug.Log($"itemsToPlace " + itemsToPlace);
                        }
                    }
                    else
                    {
                        int emptyPosition = GetEmptyPosition();
                        int activeItems = playerInteraction.PlayerTray.GetActivePositionValue(CurrentItemContainer);
                        int itemsToPlace = Mathf.Min(emptyPosition, activeItems);

                        if (itemsToPlace > 0)
                        {
                            ActivateItems(itemsToPlace);
                            playerInteraction.PlayerTray.PutAway(CurrentItemContainer, itemsToPlace);
                        }
                    }
                }
            }
            else
            {
                Debug.Log($"Current Draggable  And Tray NULL");
            }
        }
    }
}