using System.Collections;
using OrdersContent;
using SpawnContent;
using UnityEngine;

namespace ClientsContent
{
    public class ClientsCreator : MonoBehaviour
    {
        [SerializeField] private ClientsSpawner _clientsSpawner;
        [SerializeField] private OrderCreator _orderCreator;
        [SerializeField] private QueueCashRegister _queueCashRegister;

        private void Start()
        {
            // CreateClients();
        }

        [ContextMenu("Create New Client")]
        
        private void CreateClients()
        {
            StartCoroutine(Create());
        }

        private IEnumerator Create()
        {
            yield return new WaitForSeconds(1f);
            Client client = _clientsSpawner.SpawnRandomClient();
            client.Init(_orderCreator.CreateOrder());
            _queueCashRegister.AddClientToQueue(client);
        }
    }
}