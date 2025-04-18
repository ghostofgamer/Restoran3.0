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

        private void CreateClients()
        {
            Client client = _clientsSpawner.SpawnRandomClient();
        }
    }
}