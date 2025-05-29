using Enums;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerPacking : MonoBehaviour
    {
        [SerializeField] private FryerTool[] _fryerTools;
        [SerializeField] private Transform[] _positions;

        public void Packing(ItemBasket itemBasket)
        {
            Debug.Log("itemBasket " + itemBasket);

            FryerTool compatibleTool = GetCompatibleFryerTool(itemBasket.ItemType);

            if (compatibleTool != null)
            {
                Debug.Log("Item type is compatible with fryer tool: " + compatibleTool.name);
                int emptyPosition = compatibleTool.GetCountInactiveItems();
                int activeItems = itemBasket.GetActiveValueItems();

                if (emptyPosition > 0 && activeItems > 0)
                {
                    int itemsToPlace = Mathf.Min(emptyPosition, activeItems);

                    Debug.Log("emptyPosition " + emptyPosition);
                    Debug.Log("activeItems " + activeItems);

                    compatibleTool.ActivateItems(itemsToPlace);
                    itemBasket.TransferProduct(itemsToPlace, compatibleTool.Positions);
                    // targetContainer.ActivateItems(itemsToPlace);
                }
            }
            else
            {
                Debug.Log("Item type is not compatible with any of the fryer tools.");
            }
        }

        private FryerTool GetCompatibleFryerTool(ItemType itemType)
        {
            foreach (var fryerTool in _fryerTools)
            {
                if (fryerTool.ItemType == itemType)
                {
                    return fryerTool;
                }
            }

            return null;
        }
    }
}