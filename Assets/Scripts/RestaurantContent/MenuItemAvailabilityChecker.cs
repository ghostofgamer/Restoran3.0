using Enums;
using UnityEngine;

namespace RestaurantContent
{
    public class MenuItemAvailabilityChecker : MonoBehaviour
    {
        [SerializeField] private GameObject _equipmentObject;
        [SerializeField] private EquipmentType _equipmentType;

        public EquipmentType EquipmentType => _equipmentType;
        public GameObject EquipmentObject => _equipmentObject;

        public bool GetEquipmentAvailable()
        {
            if (!_equipmentObject)
                return true;

            return _equipmentObject.activeSelf;
        }
    }
}