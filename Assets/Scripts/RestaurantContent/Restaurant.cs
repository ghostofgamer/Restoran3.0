using ClientsContent;
using OrdersContent;
using RestaurantContent.TableContent;
using RestorantContent;
using UnityEngine;

namespace RestaurantContent
{
    public class Restaurant : MonoBehaviour
    {
        [SerializeField] private MenuCounter _menuCounter;
        [SerializeField] private QueueCashRegister _queueCashRegister;
        [SerializeField] private TablesCounter _tablesCounter;
        [SerializeField] private OrdersCounter _ordersCounter;

        public void AcceptOrder(Order order)
        {
            _queueCashRegister.ClientFinishedOrder();
            _ordersCounter.AddOrder(order);
        }
    }
}