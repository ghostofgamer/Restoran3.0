using System;
using System.Collections;
using Enums;
using OrdersContent;
using RestaurantContent;
using RestaurantContent.CashRegisterContent;
using RestaurantContent.TableContent;
using RestaurantContent.TrayContent;
using UnityEngine;
using UnityEngine.AI;
using WalletContent;

namespace ClientsContent
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private NavMeshObstacle _meshObstacle;
        [SerializeField] private Animator _animator;
        [SerializeField] private CashRegister _cashRegister;
        [SerializeField] private Transform _trayPositionHand;
        [SerializeField] private Collider _clientCollider;
        
        private PriceOrderCounter _priceOrderCounter;
        private Restaurant _restaurant;
        private Action<Client> _reachAction;
        private ClientState _currentState;
        private Coroutine _coroutine;
        private Transform _exitPosition;
        private QueueCashRegister _queueCashRegister;
        private ClientCar _clientCar;
        private ClientsCounter _clientsCounter;
        
        public DollarValue PriceOrder{ get; private set; }

        public DollarValue Cash { get; private set; }

        public Order Order { get; private set; }

        public Table Table { get; private set; }

        public void Init(Order order, Restaurant restaurant, Table table, Transform exitPosition,
            CashRegister cashRegister, QueueCashRegister queueCashRegister, PriceOrderCounter priceOrderCounter,
            ClientsCounter clientsCounter)
        {
            Order = order;
            PriceOrder = priceOrderCounter.GetPriceOrder(Order);
            _restaurant = restaurant;
            _currentState = ClientState.InQueue;
            Table = table;
            Order.SetTable(Table.Index);
            _exitPosition = exitPosition;
            _cashRegister = cashRegister;
            _queueCashRegister = queueCashRegister;
            _clientsCounter = clientsCounter;
            _clientsCounter.AddClient(this);
            // _clientCar = null;
            
            Cash = new DollarValue(0, 0);
            Cash = priceOrderCounter.GetCash(PriceOrder);
            // InitCash(PriceOrder);
        }

        public void SetCar(ClientCar clientCar)
        {
            _clientCar = clientCar;
        }

        public void UpdateGotoQueue()
        {
            _queueCashRegister.UpdateQueuePositions();
        }

        public void GoToQueuePosition(Vector3 position, int index)
        {
            _navMeshAgent.enabled = true;
            // _meshObstacle.enabled = true;

            if (index == 0)
            {
                Debug.Log("1");

                _currentState = ClientState.AtCashier;

                SetDestination(_cashRegister.ClientPosition.position, () =>
                {
                    Debug.Log("Дошел до кассы");
                    _cashRegister.SetClient(this);
                    _navMeshAgent.enabled = false;
                    // _meshObstacle.enabled = false;
                });
            }
            else
            {
                SetDestination(position, () =>
                {
                    _navMeshAgent.enabled = false;
                    // _meshObstacle.enabled = false;
                });
            }
        }

        public void OrderCompleted(Tray tray)
        {
            // StartCoroutine(PickUpOrder(tray));
            GoToOrderTray(tray);
        }

        /*private void InitCash(DollarValue dollarValuePriceOrder)
        {
            Cash = dollarValuePriceOrder.Dollars switch
            {
                < 10 => new DollarValue(10, 0),
                < 20 => new DollarValue(20, 0),
                < 30 => new DollarValue(30, 0),
                < 40 => new DollarValue(40, 0),
                < 50 => new DollarValue(50, 0),
                < 60 => new DollarValue(60, 0),
                < 70 => new DollarValue(70, 0),
                < 80 => new DollarValue(80, 0),
                < 90 => new DollarValue(90, 0),
                < 100 => new DollarValue(100, 0),
                _ => Cash
            };
        }*/

        private void GoToOrderTray(Tray tray)
        {
            _currentState = ClientState.PickUpOrder;
            // Debug.Log("GoToOrderTray");

            SetDestination(tray.transform.position, () =>
            {
                // Debug.Log("Дошенл до подноса ");
                GoToTableWithTray(tray);
            });
        }

        private void GoToTableWithTray(Tray tray)
        {
            tray.transform.parent = this.transform;
            tray.transform.position = _trayPositionHand.position;
            tray.transform.localRotation = _trayPositionHand.localRotation;

            _restaurant.RemoveClientTray(tray);
            _currentState = ClientState.Eat;

            // _navMeshAgent.SetDestination(Table.ClientPosition.transform.position);

            SetDestination(Table.ClientSitPosition.transform.position, () =>
            {
                _navMeshAgent.enabled = false;
                transform.position = Table.ClientSitPosition.transform.position;
                transform.rotation = Table.ClientSitPosition.transform.rotation;
                _animator.SetBool("Sit", true);
                // Debug.Log("Вернулся за стол с едой");
                Eat(tray);
            });
        }

        private void Eat(Tray tray)
        {
            Debug.Log("ЕМ");
            _currentState = ClientState.Eat;
            Debug.Log("Клиент " + gameObject.name + " вернулся за стол.");
            tray.transform.parent = Table.transform;
            tray.transform.position = Table.TrayPosition.position;
            tray.transform.rotation = Table.TrayPosition.rotation;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(EatOrder(tray));
        }

        private IEnumerator EatOrder(Tray tray)
        {
            _animator.SetBool("Eat", true);
            yield return new WaitForSeconds(6f);
            _animator.SetBool("Eat", false);
            tray.SetActivity(false);
            tray.DefaultReturn();

            GoAway();
        }

        private void GoAway()
        {
            Table.DirtyTable();
            Table.SetBusyValue(false);
            _animator.SetBool("Sit", false);
            _currentState = ClientState.GoAway;
            _clientsCounter.RemoveClient(this);
            
            if (_clientCar != null)
            {
                SetDestination(_clientCar.ExitPosition.position, () =>
                {
                    _clientCar.RemoveClient(this);
                    _clientCar = null;
                    gameObject.SetActive(false);
                });
            }
            else
            {
                SetDestination(_exitPosition.transform.position, () => { gameObject.SetActive(false); });
            }
        }

        [ContextMenu("Completed")]
        public void Paid()
        {
            Debug.Log("Paid");
            _currentState = ClientState.WaitingForOrder;
            _navMeshAgent.enabled = true;
            // _meshObstacle.enabled = true;

            /*if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartPaid());*/


            SetDestination(Table.ClientSitPosition.transform.position, () =>
            {
                _navMeshAgent.enabled = false;
                _animator.SetBool("Sit", true);
                transform.position = Table.ClientSitPosition.transform.position;
                transform.rotation = Table.ClientSitPosition.transform.rotation;
                Debug.Log("Жду за столом ");
            });
        }

        private IEnumerator StartPaid()
        {
            _animator.SetBool("Give", true);
            yield return new WaitForSeconds(0.5f);

            // _currentState = ClientState.WaitingForOrder;
            // Debug.Log("Пошел ждать заказ");
            _animator.SetBool("Give", false);

            SetDestination(Table.ClientSitPosition.transform.position, () =>
            {
                _navMeshAgent.enabled = false;
                _animator.SetBool("Sit", true);
                transform.position = Table.ClientSitPosition.transform.position;
                transform.rotation = Table.ClientSitPosition.transform.rotation;
                Debug.Log("Жду за столом ");
            });
        }

        public void SetDestination(Vector3 position, System.Action callback)
        {
            /*_navMeshAgent.enabled = true;
            _meshObstacle.enabled = true;*/

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(MoveToPosition(position, callback));
        }

        private IEnumerator MoveToPosition(Vector3 position, System.Action callback)
        {
            // _meshObstacle.enabled = false;
            // _clientCollider.enabled = false;
            _meshObstacle.enabled = true;
            
            if (!_navMeshAgent.enabled)
            {
                _navMeshAgent.enabled = true;
                transform.position = Table.ClientStandPosition.position;
                transform.rotation = Table.ClientStandPosition.rotation;
            }

            if (_animator.GetBool("Sit"))
                _animator.SetBool("Sit", false);

            _navMeshAgent.SetDestination(position);

            _animator.SetBool(_currentState == ClientState.Eat ? "WalkTray" : "Walking", true);

            while (_navMeshAgent.pathPending)
                yield return null;

            while (_navMeshAgent.remainingDistance > 0.1f)
                yield return null;

            _meshObstacle.enabled = false;
            // _meshObstacle.enabled = true;
            // _clientCollider.enabled = true;
            Debug.Log("Завершил идти ");

            _animator.SetBool(_currentState == ClientState.Eat ? "WalkTray" : "Walking", false);
            callback.Invoke();
        }
        
        public bool CanInteractWithCashier()
        {
            return _currentState == ClientState.AtCashier;
        }
    }
}