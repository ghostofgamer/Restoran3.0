using RestaurantContent.CashRegisterContent;
using UnityEngine;

namespace UI.Buttons.CashRegisterButtons
{
    public class ClearGivingValueButton : AbstractButton
    {
        [SerializeField] private CashRegister _cashRegister;
        
        public override void OnClick()
        {
            _cashRegister.ClearGivingValue();
        }
    }
}