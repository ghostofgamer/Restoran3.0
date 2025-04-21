using ClientsContent;
using OrdersContent;
using RestaurantContent.TableContent;
using RestaurantContent.TrayContent;
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
        [SerializeField] private TrayCounter _trayCounter;

        public void AcceptOrder(Order order, Client client)
        {
            _queueCashRegister.ClientFinishedOrder();
            _ordersCounter.AddOrder(order, client);
        }

        public void RemoveClientTray(Tray tray)
        {
            _trayCounter.UpdateTrayList(tray);
        }
    }
}