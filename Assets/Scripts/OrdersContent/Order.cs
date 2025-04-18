using System;
using Enums;

namespace OrdersContent
{
    [Serializable]
    public class Order 
    {
        private ItemType _burgerItemOrder;
        private ItemType _drinkItemOrder;
        private ItemType _extraItemOrder;

        public Order(ItemType burgerItemOrder,ItemType drinkItemOrder,ItemType extraItemOrder)
        {
            _burgerItemOrder = burgerItemOrder;
            _drinkItemOrder = drinkItemOrder;
            _extraItemOrder = extraItemOrder;
        }
    }
}