using ClientsContent;
using UnityEngine;

namespace RestaurantContent
{
    public class CashierTrigger : MonoBehaviour
    {
        [SerializeField] private Restaurant _restaurant;
        
        private void OnTriggerEnter(Collider other)
        {
            Client client = other.GetComponent<Client>();
        
            if (client != null && client.CanInteractWithCashier())
            {
                client.Paid();
                _restaurant.AcceptOrder();
            }
        }
    }
}