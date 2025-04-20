using System.Data;
using OrdersContent;
using TMPro;
using UnityEngine;

namespace RestaurantContent
{
    public class Tray : MonoBehaviour
    {
        [SerializeField] private TMP_Text _indexTable;
        [SerializeField] private OrdersCounter _ordersCounter;
        [SerializeField] private Transform _clientPosition;

        private Order _order;

        public bool IsBusy { get; private set; }

        public void SetBusy(bool value)
        {
            IsBusy = value;
        }

        public void SetOrder(Order order)
        {
            _order = order;
            _indexTable.gameObject.SetActive(true);
            _indexTable.text = (order.IndexTable + 1).ToString();
        }

        [ContextMenu("Completed")]
        public void Completed()
        {
            _ordersCounter.CompleteOrder(_order, transform);
        }
    }
}