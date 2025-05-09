using System.Collections.Generic;
using OrdersContent;
using UI.Screens;
using UnityEngine;

namespace RestaurantContent
{
    public class KitchenOrdersScreen : MonoBehaviour
    {
        [SerializeField] private OrdersCounter _ordersCounter;
        [SerializeField] private OrderUIScreen[] _orderUiScreens;

        private void OnEnable()
        {
            _ordersCounter.OrdersChanged += UpdateScreenOrders;
            _ordersCounter.UpdateOrders += UpdateOrders;
        }

        private void OnDisable()
        {
            _ordersCounter.OrdersChanged -= UpdateScreenOrders;
            _ordersCounter.UpdateOrders -= UpdateOrders;
        }

        private void UpdateScreenOrders(List<Order> orders)
        {
            Debug.Log("orders " + orders.Count);
            
            foreach (var orderScreen in _orderUiScreens)
            {
                orderScreen.Deactivate();
                orderScreen.gameObject.SetActive(false);
            }
            Debug.Log("1 " + orders.Count);
            if (orders.Count <= 0)
                return;
            Debug.Log("3 " + orders.Count);
            for (int i = 0; i < orders.Count; i++)
            {
                if (i < _orderUiScreens.Length)
                {
                    _orderUiScreens[i].gameObject.SetActive(true);
                    _orderUiScreens[i].Init(orders[i]);
                }
            }
            Debug.Log("5 " + orders.Count);
        }

        private void UpdateOrders(int value )
        {
            if (value == 0)
            {
                foreach (var orderScreen in _orderUiScreens)
                {
                    orderScreen.Deactivate();
                    orderScreen.gameObject.SetActive(false);
                }
            }
            Debug.Log("value " + value);
        } 
    }
}