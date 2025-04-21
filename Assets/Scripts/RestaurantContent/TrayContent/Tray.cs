using OrdersContent;
using TMPro;
using UnityEngine;

namespace RestaurantContent.TrayContent
{
    public class Tray : MonoBehaviour
    {
        [SerializeField] private GameObject _check;
        [SerializeField] private TMP_Text _indexTable;
        [SerializeField] private OrdersCounter _ordersCounter;
        [SerializeField] private Transform _defaultParent;

        private Order _order;

        public bool IsBusy { get; private set; }

        public void Init(OrdersCounter ordersCounter,Transform defaultParent)
        {
            _ordersCounter = ordersCounter;
            _defaultParent = defaultParent;
        }
        
        public void SetBusy(bool value)
        {
            IsBusy = value;
        }

        public void SetOrder(Order order)
        {
            _order = order;
            _check.SetActive(true);
            _indexTable.text = (order.IndexTable + 1).ToString();
        }

        [ContextMenu("Completed")]
        public void Completed()
        {
            _ordersCounter.CompleteOrder(_order, this);
        }

        public void Clear()
        {
            SetBusy(false);
            _order = null;
            _check.SetActive(false);
        }

        public void DefaultReturn()
        {
            Debug.Log("Вернули в пул");
            Clear();
            transform.parent = _defaultParent;
        }

        public void SetActivity(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}