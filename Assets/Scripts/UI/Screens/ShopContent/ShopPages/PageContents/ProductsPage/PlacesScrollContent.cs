using System;
using PlayerContent.LevelContent;
using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage
{
    public class PlacesScrollContent : PageScrollContent
    {
        [SerializeField] private PlaceUIProduct[] _placeUIProducts;
        [SerializeField] private PlayerLevel _playerLevel;

        [SerializeField] private PlaceUIProduct[] _placeTaskUIProductsPurchasingPossibility;
        public event Action<int> PayPlaceCompleted;

        public PlaceUIProduct[] PlaceUIProductsPayPossibility => _placeTaskUIProductsPurchasingPossibility;

        public override void Init()
        {
            int value = 0;

            foreach (var placeUIProduct in _placeUIProducts)
            {
                placeUIProduct.ShowIndexPosition(value);
                value++;
                placeUIProduct.Init();
            }
            
            Debug.Log("GasmeObj " + gameObject.name);
        }

        public void PayPlace(int placeTableIndex)
        {
            PayPlaceCompleted?.Invoke(placeTableIndex);
        }
    }
}