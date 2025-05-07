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
        [SerializeField] private GameObject _canvas;

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
            _canvas.SetActive(_currentClient != null);
        }

        [ContextMenu("AcceptOrder")]
        private void AcceptOrder(PlayerInteraction playerInteraction)
        {
            if (_currentClient == null)
                return;
       
            /*if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartAcceptOrder());*/
            
            _currentClient.Paid();
            Client client = _currentClient;
            _currentClient = null;
            _canvas.SetActive(_currentClient != null);
            _restaurant.AcceptOrder(client.Order, client);
        }

        private IEnumerator StartAcceptOrder()
        {
            _currentClient.Paid();
            Client client = _currentClient;
            _currentClient = null;
            _canvas.SetActive(_currentClient != null);
            yield return new WaitForSeconds(0.6f);
            _restaurant.AcceptOrder(client.Order, client);
            // _currentClient = null;
        }
    }
}