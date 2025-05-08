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

        public DollarValue GetCash(DollarValue dollarValuePriceOrder)
        {
            return dollarValuePriceOrder.Dollars switch
            {
                < 10 => new DollarValue(10, 0),
                < 20 => new DollarValue(20, 0),
                < 30 => new DollarValue(30, 0),
                < 40 => new DollarValue(40, 0),
                < 50 => new DollarValue(50, 0),
                < 60 => new DollarValue(60, 0),
                < 70 => new DollarValue(70, 0),
                < 80 => new DollarValue(80, 0),
                < 90 => new DollarValue(90, 0),
                < 100 => new DollarValue(100, 0),
                _ => dollarValuePriceOrder
            };
        }

        public DollarValue GetChange(DollarValue price, DollarValue cash)
        {
            int totalCents = cash.ToTotalCents() - price.ToTotalCents();
            Debug.Log("totalCents" + totalCents);
            DollarValue change = new DollarValue(0,0).FromTotalCents(totalCents);
            Debug.Log("6" + change.ToString());

            return change;
        }
    }
}