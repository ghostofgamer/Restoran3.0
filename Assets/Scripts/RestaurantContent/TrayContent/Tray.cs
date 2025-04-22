using System.Collections.Generic;
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
        [SerializeField] private Transform[] _itemPositions;

        private List<Item> _items = new List<Item>();

        public Order Order { get; private set; }

        public bool IsBusy { get; private set; }

        public Transform[] ItemPositions => _itemPositions;

        public void Init(OrdersCounter ordersCounter, Transform defaultParent)
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
            Order = order;
            _check.SetActive(true);
            _indexTable.text = (order.IndexTable + 1).ToString();
        }

        [ContextMenu("Completed")]
        public void Completed()
        {
            _ordersCounter.CompleteOrder(Order, this);
        }

        public void Clear()
        {
            SetBusy(false);
            Order = null;
            _check.SetActive(false);
        }

        public void DefaultReturn()
        {
            foreach (var item in _items)
                item.ReturnDefaultParent();
            
            _items.Clear();

            Debug.Log("Вернули в пул");
            Clear();
            transform.parent = _defaultParent;
        }

        public void SetActivity(bool value)
        {
            gameObject.SetActive(value);
        }

        public void SetBurger(Item item)
        {
            _items.Add(item);
            Order.SetBurgerCompleted(true);
        }

        public void TryCompletedOrder()
        {
            if (Order.IsOrderCompleted())
            {
                Debug.Log("Заказ завершен");
                Completed();
            }
        }

        public Transform GetFirstAvailablePosition()
        {
            foreach (var position in _itemPositions)
            {
                if (position.childCount == 0)
                    return position;
            }

            return null;
        }
    }
}