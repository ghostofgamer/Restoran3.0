using System.Collections.Generic;
using System.Linq;
using ClientsContent;
using RestaurantContent;
using UnityEngine;

namespace OrdersContent
{
    public class OrdersCounter : MonoBehaviour
    {
        [SerializeField] private Tray[] _trays;
        [SerializeField] private List<Client> _activeOrderWaitClients;

        private const int MaxActiveOrders = 4;

        private List<Order> _currentOrders;
        private Queue<Order> _orderQueue;

        private void Start()
        {
            _currentOrders = new List<Order>();
            _orderQueue = new Queue<Order>();
        }

        public void AddOrder(Order order, Client client)
        {
            if (_currentOrders.Count < MaxActiveOrders)
            {
                _currentOrders.Add(order);
                _activeOrderWaitClients.Add(client);
                UpdateTrays(order);
                Debug.Log("Добавлен новый активный заказ: " + order.IndexTable);
            }
            else
            {
                _orderQueue.Enqueue(order);
                Debug.Log("Добавлен новый заказ в очередь: " + order.IndexTable);
            }
        }

        public void CompleteOrder(Order order,Transform positionTray)
        {
            if (_currentOrders.Contains(order))
            {
                _currentOrders.Remove(order);
                
                Client client = _activeOrderWaitClients.FirstOrDefault(c => c.Order == order);
                
                if (client != null)
                {
                    _activeOrderWaitClients.Remove(client);
                    client.OrderCompleted(positionTray);
                }
                
                TryActivateOrder();
            }
        }

        private void TryActivateOrder()
        {
            while (_currentOrders.Count < MaxActiveOrders && _orderQueue.Count > 0)
            {
                Order nextOrder = _orderQueue.Dequeue();
                _currentOrders.Add(nextOrder);
                UpdateTrays(nextOrder);
                Debug.Log("Активирован новый заказ: " + nextOrder.IndexTable);
            }
        }

        private void UpdateTrays(Order order)
        {
            int freeTraysCount = _trays.Count(tray => !tray.IsBusy);
            Debug.Log("Количество свободных подносов: " + freeTraysCount);
            Tray freeTray = _trays.FirstOrDefault(tray => !tray.IsBusy);

            if (freeTray != null)
            {
                freeTray.SetBusy(true);
                freeTray.SetOrder(order);
            }
        }
    }
}