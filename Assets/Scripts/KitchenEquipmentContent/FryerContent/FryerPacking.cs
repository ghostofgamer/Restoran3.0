using Enums;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerPacking : MonoBehaviour
    {
        [SerializeField] private FryerTool[] _fryerTools;
        
        public void Packing(ItemBasket itemBasket)
        {
            Debug.Log("itemBasket " + itemBasket);
            
            FryerTool compatibleTool = GetCompatibleFryerTool(itemBasket.ItemType);

            if (compatibleTool != null)
            {
                Debug.Log("Item type is compatible with fryer tool: " + compatibleTool.name);
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