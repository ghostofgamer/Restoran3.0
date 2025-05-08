using RestaurantContent.CashRegisterContent;
using UnityEngine;

namespace UI.Buttons
{
    public class ChangeGivingValueButton : AbstractButton
    {
        [SerializeField] private int _valueCents;
        [SerializeField] private CashRegister _cashRegister;
        
        public override void OnClick()
        {
            _cashRegister.ChangeGivingValue(_valueCents);
        }
    }
}