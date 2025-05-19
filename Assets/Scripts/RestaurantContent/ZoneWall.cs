using UI.Screens.ShopContent;
using UnityEngine;

namespace RestaurantContent
{
    public class ZoneWall : MonoBehaviour
    {
        [SerializeField] private ZoneUIProduct _zoneUIProduct;
        [SerializeField] private GameObject _closeDoor;
        [SerializeField] private GameObject _openDoor;
        [SerializeField] private GameObject _zoneEnvironment;
        [SerializeField] private bool _isDoor;

        private void Start()
        {
            Activate();
        }

        public void Activate()
        {
            if (_isDoor)
            {
                _closeDoor.SetActive(!_zoneUIProduct.IsBuyed());
                _openDoor.SetActive(_zoneUIProduct.IsBuyed());
                _zoneEnvironment.SetActive(_zoneUIProduct.IsBuyed());
            }
            else
            {
                gameObject.SetActive(!_zoneUIProduct.IsBuyed());
            }
        }
    }
}