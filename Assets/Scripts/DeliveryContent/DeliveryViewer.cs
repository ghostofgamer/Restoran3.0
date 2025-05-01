using TMPro;
using UnityEngine;

namespace DeliveryContent
{
    public class DeliveryViewer : MonoBehaviour
    {
        [SerializeField] private GameObject _deliveryScreen;
        [SerializeField] private TMP_Text _timerDeliveryText;
        [SerializeField] private TMP_Text _amountDelivers;
        [SerializeField] private Delivery _delivery;

        private void OnEnable()
        {
            _delivery.AmountItemsDeliveriesChanged += ShowAmountDeliveries;
        }

        private void OnDisable()
        {
            _delivery.AmountItemsDeliveriesChanged -= ShowAmountDeliveries;
        }

        private void ShowAmountDeliveries(int amount)
        {
            _amountDelivers.text = amount.ToString();
        }
    }
}