using Enums;
using UnityEngine;

namespace KitchenEquipmentContent.FryerContent
{
    public class FryerTool : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType;

        public ItemType ItemType => _itemType;
    }
}