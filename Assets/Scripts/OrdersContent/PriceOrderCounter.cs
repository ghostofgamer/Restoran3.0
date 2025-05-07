using Enums;
using SoContent;
using UnityEngine;
using WalletContent;

namespace OrdersContent
{
    public class PriceOrderCounter : MonoBehaviour
    {
        private const string CurrentPriceKey = "CurrentPrice";

        [SerializeField] private ItemsConfig _itemsConfig;

        public DollarValue Price;

        public DollarValue GetPriceOrder(Order order)
        {
            int burgerTotalCents = 0;

            if (order.BurgerItemOrder != ItemType.Empty)
            {
                burgerTotalCents = PlayerPrefs.GetInt(CurrentPriceKey + order.BurgerItemOrder,
                    _itemsConfig.GetItemConfig(order.BurgerItemOrder).RecommendedPrice.ToTotalCents());
            }

            int drinkTotalCents = 0;

            if (order.DrinkItemOrder != ItemType.Empty)
            {
                drinkTotalCents = PlayerPrefs.GetInt(CurrentPriceKey + order.DrinkItemOrder,
                    _itemsConfig.GetItemConfig(order.DrinkItemOrder).RecommendedPrice.ToTotalCents());
            }

            int extraTotalCents = 0;

            if (order.ExtraItemOrder != ItemType.Empty)
            {
                extraTotalCents = PlayerPrefs.GetInt(CurrentPriceKey + order.ExtraItemOrder,
                    _itemsConfig.GetItemConfig(order.ExtraItemOrder).RecommendedPrice.ToTotalCents());
            }
            
            int totalCents = burgerTotalCents + drinkTotalCents + extraTotalCents;
            DollarValue price = new DollarValue(0, 0).FromTotalCents(totalCents);
            
            
            return price;
        }
    }
}