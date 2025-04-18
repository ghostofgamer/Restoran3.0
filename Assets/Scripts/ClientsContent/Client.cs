using OrdersContent;
using UnityEngine;

namespace ClientsContent
{
    public class Client : MonoBehaviour
    {
        private Order _order;

        public void Init(Order order)
        {
            _order = order;
        }
    }
}