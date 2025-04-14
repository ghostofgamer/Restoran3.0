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

                if (basket != null)
                {
                    int emptyPosition = GetEmptyPosition();
                    int activeItems = basket.GetActiveValueItems();

                    if (emptyPosition > 0 && activeItems > 0)
                    {
                        int itemsToPlace = Mathf.Min(emptyPosition, activeItems);
                        basket.TransferProduct(itemsToPlace,Positions);
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
            else
            {
                Debug.Log($"Current Draggable  NULL");
            }
        }
    }
}
