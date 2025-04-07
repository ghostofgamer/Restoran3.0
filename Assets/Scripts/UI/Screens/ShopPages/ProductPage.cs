using UnityEngine;

namespace UI.Screens
{
    public class ProductPage : ShopPage
    {
        [SerializeField] private GameObject _halfBackgroundImage;
        [SerializeField] private GameObject _fullBackgroundImage;

        public override void Open(int index)
        {
            base.Open(index);
            
            if (index == 0)
            {
                _halfBackgroundImage.SetActive(true); 
                _fullBackgroundImage.SetActive(false); 
            }
            else
            {
                _halfBackgroundImage.SetActive(false); 
                _fullBackgroundImage.SetActive(true); 
            }
        }
    }
}