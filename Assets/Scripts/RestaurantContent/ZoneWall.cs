using UI.Screens.ShopContent;
using UnityEngine;

namespace RestaurantContent
{
    public class ZoneWall : MonoBehaviour
    {
        [SerializeField] private ZoneUIProduct _zoneUIProduct;

        private void Start()
        {
            gameObject.SetActive(!_zoneUIProduct.IsBuyed());
        }
    }
}