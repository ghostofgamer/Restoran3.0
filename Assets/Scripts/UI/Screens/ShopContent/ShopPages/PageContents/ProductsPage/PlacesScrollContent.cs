using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage
{
    public class PlacesScrollContent : PageScrollContent
    {
        [SerializeField] private PlaceUIProduct[] _placeUIProducts;
        
        public override void Init()
        {
            foreach (var placeUIProduct in _placeUIProducts)
                placeUIProduct.Init();
            
            Debug.Log("GasmeObj " + gameObject.name);
        }
    }
}
