using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages
{
    public class PageContent : MonoBehaviour
    {
        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        } 
    }
}
