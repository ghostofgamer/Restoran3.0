using ClientsContent;
using InteractableContent;
using PlayerContent;
using UnityEngine;

namespace RestaurantContent.CashRegisterContent
{
    public class CashRegister : MonoBehaviour
    {
        [SerializeField] private Restaurant _restaurant;
        [SerializeField] private InteractableObject _interactableObject;

        private Client _currentClient;

        private void OnEnable()
        {
            _interactableObject.OnAction += AcceptOrder;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= AcceptOrder;
        }

        public void SetClient(Client client)
        {
            _currentClient = client;
        }
        
        private void AcceptOrder(PlayerInteraction playerInteraction)
        {
            if (_currentClient == null)
                return;
            
            _currentClient.Paid();
            _currentClient = null;
            _restaurant.AcceptOrder();
        }
    }
}
