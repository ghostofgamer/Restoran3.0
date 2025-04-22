using ClientsContent;
using Enums;
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

        public bool TryGetTrayOrder(ItemType itemType, out Tray tray)
        {
            Order order = _ordersCounter.GetOrderByBurger(itemType);
            Debug.Log("Order " + (order?.IndexTable + 1));
            tray = null;

            if (order != null)
                tray = _trayCounter.GetTrayByTableIndex(order);

            if (tray != null)
            {
                // tray.SetBurger(item);
                return true;
            }

            return false;
        }

        public void SetBurgerOrder(Tray tray, Item item)
        {
                tray.SetBurger(item);
        }
    }
}