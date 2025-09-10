using Enums;
using UI.Buttons;
using UnityEngine;

namespace IAP
{
    public class PurchaseButton : AbstractButton
    {
        [SerializeField]private Purchaser _purchaser;
        [SerializeField]private IapProductType _productType;
    
        public override void OnClick()
        {
            _purchaser.BuyProduct(_productType);
        }
    }
}