using System.Collections;
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
        private Coroutine _coroutine;
        
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
        
        [ContextMenu("AcceptOrder")]
        private void AcceptOrder(PlayerInteraction playerInteraction)
        {
            if (_currentClient == null)
                return;
            
            if(_coroutine!=null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartAcceptOrder());
            
            /*_currentClient.Paid();
            _restaurant.AcceptOrder(_currentClient.Order,_currentClient);
            _currentClient = null;*/
        }
        
        private IEnumerator StartAcceptOrder()
       {
           _currentClient.Paid();
           
           yield return new WaitForSeconds(1f);
           _restaurant.AcceptOrder(_currentClient.Order,_currentClient);
           _currentClient = null;
       }
    }
}
