using ClientsContent;
using OrdersContent;
using TMPro;
using UnityEngine;
using WalletContent;

namespace RestaurantContent.CashRegisterContent
{
    public class CashRegisterViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _totalText;
        [SerializeField] private TMP_Text _receivedText;
        [SerializeField] private TMP_Text _changeText;
        [SerializeField] private TMP_Text _givingText;
        [SerializeField] private PriceOrderCounter _priceOrderCounter;
        [SerializeField] private GameObject _panelOrder;
        [SerializeField] private GameObject _panelGivingChange;
        [SerializeField] private CashRegister _cashRegister;

        private DollarValue _currentChangeValue;

        private void OnEnable()
        {
            _cashRegister.GivingValueChanged += ShowGivingValueText;
        }

        private void OnDisable()
        {
            _cashRegister.GivingValueChanged -= ShowGivingValueText;
        }

        public void SetValuePanels(bool value)
        {
            _panelOrder.SetActive(!value);
            _panelGivingChange.SetActive(value);
        }

        public void Init(Client client, DollarValue givingValue)
        {
            _currentChangeValue = _priceOrderCounter.GetChange(client.PriceOrder, client.Cash);
            _totalText.text = $"Total:{client.PriceOrder}";
            _receivedText.text = $"Received:{client.Cash}";
            _changeText.text = $"Change:<color=yellow>{_currentChangeValue + "</color>"}";
            // _givingText.text = "Giving:<color=red> $0.00</color>";
            ShowGivingValueText(givingValue);
        }

        private void ShowGivingValueText(DollarValue dollarValue)
        {
            Debug.Log("CHANGEEEE " + dollarValue.ToString());

            if (dollarValue.ToTotalCents() == _currentChangeValue.ToTotalCents())
            {
                Debug.Log("ОДИНАКОВЫЕ ");
            }
            else
            {
                Debug.Log("Разные " );
            } 

            string colorHex = (dollarValue.ToTotalCents() == _currentChangeValue.ToTotalCents())
                ? "#00FF00"
                : "#FF0000";
            _givingText.text = $"Giving:<color={colorHex}> {dollarValue.ToString()}</color>";
        }
    }
}