using System.Collections;
using OrdersContent;
using RestaurantContent;
using RestaurantContent.CashRegisterContent;
using RestaurantContent.TableContent;
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
        [SerializeField] private TablesCounter _tablesCounter;
        [SerializeField] private Transform _exitPosition;
        [SerializeField] private CashRegister _cashRegister;

        [ContextMenu("Create New Client")]
        public void CreateClients()
        {
            if (_queueCashRegister.IsQueueFull())
            {
                Debug.Log("Очередь заполнена");
                return;
            }

            if (_tablesCounter.GetFreeTableCount() <= 0)
            {
                Debug.Log("Свободных столов нету");
                return;
            }
            
            StartCoroutine(Create());
        }

        private IEnumerator Create()
        {
            Table table = _tablesCounter.GetAvailableTable();
            table.SetBusyValue(true);
            Client client = _clientsSpawner.SpawnRandomClient();
            client.Init(_orderCreator.CreateOrder(),_restaurant,table,_exitPosition,_cashRegister);
            _queueCashRegister.AddClientToQueue(client);
            yield return null;
        }
    }
}