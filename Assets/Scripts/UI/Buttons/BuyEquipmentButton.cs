using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class BuyEquipmentButton : AbstractButton
    {
        [SerializeField] private EquipmentUIProduct _equipmentUIProduct;
        
        public override void OnClick()
        {
            _equipmentUIProduct.Buy();
        }
    }
}