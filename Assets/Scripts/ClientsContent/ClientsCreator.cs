using System.Collections;
using OrdersContent;
using RestorantContent;
using SpawnContent;
using UnityEngine;

namespace ClientsContent
{
    public class ClientsCreator : MonoBehaviour
    {
        [SerializeField] private ClientsSpawner _clientsSpawner;
        [SerializeField] private OrderCreator _orderCreator;
        [SerializeField] private QueueCashRegister _queueCashRegister;
        [SerializeField] private Restaurant _restaurant;

        [ContextMenu("Create New Client")]
        
        private void CreateClients()
        {
            StartCoroutine(Create());
        }

        private IEnumerator Create()
        {
            yield return new WaitForSeconds(1f);
            Client client = _clientsSpawner.SpawnRandomClient();
            client.Init(_orderCreator.CreateOrder(),_restaurant);
            _queueCashRegister.AddClientToQueue(client);
        }
    }
}