using System.Collections;
using OrdersContent;
using ParkingContent;
using RestaurantContent;
using RestaurantContent.CashRegisterContent;
using RestaurantContent.TableContent;
using SpawnContent;
using UnityEngine;
using Random = UnityEngine.Random;

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

        [SerializeField] private ClientCar _clientCar;
        [SerializeField] private Transform _carSpawnPosition;
        [SerializeField] private Transform _carParkingPosition;
        [SerializeField] private Parking _parking;

        [SerializeField] private bool _isWork = true;
        [SerializeField] private float _minTimeSpawn;
        [SerializeField] private float _maxTimeSpawn;

        private float _elapsedTime;
        private float _nextSpawnTime = 0f;
        
        private void Start()
        {
            _nextSpawnTime = Random.Range(_minTimeSpawn, _maxTimeSpawn);  
        }

        private void Update()
        {
            if (_isWork)
            {
                _elapsedTime += Time.deltaTime;
                
                if (_elapsedTime >= _nextSpawnTime)
                {
                    _elapsedTime = 0;
                    _nextSpawnTime = Random.Range(_minTimeSpawn, _maxTimeSpawn);
                    CreateClients();
                }
            }
        }

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
            client.Init(_orderCreator.CreateOrder(), _restaurant, table, _exitPosition, _cashRegister,
                _queueCashRegister);

            if (_parking.GetCountFreeParkingPositions() > 0)
            {
                float randomValue = Random.Range(0f, 1f);

                if (randomValue < 0.5f)
                {
                    Debug.Log("В машине рандом");
                    ParkingSpace parkingSpace = _parking.GetFreeParkingPosition();
                    ClientCar car = Instantiate(_clientCar, _carSpawnPosition.position, Quaternion.identity, transform);
                    parkingSpace.BusyPlace(car);
                    car.AddClient(client, parkingSpace);
                    car.GoToPosition(parkingSpace.transform.position);
                    _queueCashRegister.AddClientQueue(client);
                }
                else
                {
                    Debug.Log("В пешком рандом");
                    _queueCashRegister.AddClientToQueue(client);
                }
            }
            else
            {
                Debug.Log("пешком");
                _queueCashRegister.AddClientToQueue(client);
            }
            yield return null;
        }
    }
}