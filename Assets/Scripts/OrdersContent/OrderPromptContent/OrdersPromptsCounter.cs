using System.Collections.Generic;
using UnityEngine;

namespace OrdersContent.OrderPromptContent
{
    public class OrdersPromptsCounter : MonoBehaviour
    {
        [SerializeField] private OrderPrompt[] _orderPrompts;
        [SerializeField] private OrdersCounter _ordersCounter;

        private void OnEnable()
        {
            _ordersCounter.OrdersChanged += UpdateOrdersPrompts;
        }

        private void OnDisable()
        {
            _ordersCounter.OrdersChanged -= UpdateOrdersPrompts;
        }

        private void UpdateOrdersPrompts(List<Order> orders)
        {
            foreach (var orderPrompt in _orderPrompts)
                orderPrompt.Deactivate();

            for (int i = 0; i < orders.Count; i++)
            {
                _orderPrompts[i].InitOrder(orders[i]);
                _orderPrompts[i].Activate();
            }
        }
    }
}