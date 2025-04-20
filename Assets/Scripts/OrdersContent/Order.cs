using System;
using Enums;

namespace OrdersContent
{
    [Serializable]
    public class Order
    {
        public ItemType BurgerItemOrder { get; private set; }
        
        public ItemType DrinkItemOrder { get; private set; }
        
        public ItemType ExtraItemOrder { get; private set; }

        public int IndexTable { get; private set; }

        public Order(ItemType burgerItemOrder, ItemType drinkItemOrder, ItemType extraItemOrder)
        {
            BurgerItemOrder = burgerItemOrder;
            DrinkItemOrder = drinkItemOrder;
            ExtraItemOrder = extraItemOrder;
        }

        public void SetTable(int index)
        {
            IndexTable = index;
        }
    }
}