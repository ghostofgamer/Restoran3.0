using Enums;
using OrdersContent;
using SoContent;
using UnityEngine;
using UnityEngine.UI;

namespace ClientsContent
{
    public class IdeaOrderClient : MonoBehaviour
    {
        [SerializeField] private Image[] _images;

        /*public void Init(Order order,ItemsConfig itemsConfig)
        {
            DisableImages();

            int imageIndex = 0;
            
            TrySetSprite(order.BurgerItemOrder, itemsConfig, ref imageIndex);
            TrySetSprite(order.DrinkItemOrder, itemsConfig, ref imageIndex);
            TrySetSprite(order.ExtraItemOrder, itemsConfig, ref imageIndex);
        }*/

        public void Init(Order order, ItemsConfig itemsConfig)
        {
            DisableImages();
            TrySetSprite(order.DrinkItemOrder, itemsConfig, 0);
            TrySetSprite(order.BurgerItemOrder, itemsConfig, 1);
            TrySetSprite(order.ExtraItemOrder, itemsConfig, 2);
        }

        private void TrySetSprite(ItemType itemType, ItemsConfig itemsConfig, int imageIndex)
        {
            if (itemType != null && imageIndex < _images.Length)
            {
                ItemConfig itemConfig = itemsConfig.GetItemConfig(itemType);
                if (itemConfig != null && itemConfig.SpriteOutline != null)
                {
                    _images[imageIndex].sprite = itemConfig.SpriteOutline;
                    _images[imageIndex].gameObject.SetActive(true);
                }
            }
        }

        private void DisableImages()
        {
            foreach (var image in _images)
                image.gameObject.SetActive(false);
        }
        
        /*private void TrySetSprite(ItemType itemType, ItemsConfig itemsConfig, ref int imageIndex)
        {
            if (itemType != null && imageIndex < _images.Length)
            {
                ItemConfig itemConfig = itemsConfig.GetItemConfig(itemType);
                
                if (itemConfig != null && itemConfig.SpriteOutline != null)
                {
                    _images[imageIndex].sprite = itemConfig.SpriteOutline;
                    _images[imageIndex].gameObject.SetActive(true);
                    imageIndex++;
                }
            }
        }*/
    }
}