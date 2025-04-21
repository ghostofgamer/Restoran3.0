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
        [SerializeField] private Transform _clientPosition;

        private Client _currentClient;
        private Coroutine _coroutine;
        
        public Transform ClientPosition => _clientPosition;
        
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
            Debug.Log("3");
            if (_currentClient == null)
                return;
            Debug.Log("5");
            if(_coroutine!=null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartAcceptOrder());
            
            /*_currentClient.Paid();
            _restaurant.AcceptOrder(_currentClient.Order,_currentClient);
            _currentClient = null;*/
        }
        
        private IEnumerator StartAcceptOrder()
       { Debug.Log("6");
           _currentClient.Paid();
           Debug.Log("7");
           yield return new WaitForSeconds(1f);
           _restaurant.AcceptOrder(_currentClient.Order,_currentClient);
           _currentClient = null;
           Debug.Log("8");
       }
    }
}
