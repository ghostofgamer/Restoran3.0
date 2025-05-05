using UI.Screens.ShopContent;
using UnityEngine;

namespace UI.Buttons
{
    public class BuyPlaceTableButton : AbstractButton
    {
        [SerializeField] private PlaceUIProduct _placeUIProduct;

        public override void OnClick()
        {
            _placeUIProduct.Buy();
        }
    }
}