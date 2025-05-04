using UI.Screens.ShopContent;
using UnityEngine;

namespace UI.Buttons
{
    public class BuyZoneButton : AbstractButton
    {
        [SerializeField] private ZoneUIProduct _zoneUIProduct;
        
        public override void OnClick()
        {
            _zoneUIProduct.Buy();
        }
    }
}