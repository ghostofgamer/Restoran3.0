using RestaurantContent.CashRegisterContent;
using UnityEngine;

namespace UI.Buttons.CashRegisterButtons
{
    public class UndoChangeGivingValueButton : AbstractButton
    {
        [SerializeField] private CashRegister _cashRegister;
    
        public override void OnClick()
        {
            _cashRegister.UndoLastChange();
        }
    }
}