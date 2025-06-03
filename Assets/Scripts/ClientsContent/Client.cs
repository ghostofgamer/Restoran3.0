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
using Random = System.Random;

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
        private Coroutine _trayCoroutine;

        public DollarValue PriceOrder { get; private set; }

        public DollarValue Cash { get; private set; }

        public bool IsCard { get; private set; }

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


            Random random = new Random();
            IsCard = random.Next(2) == 1;

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
            GoToOrderTray(tray);
        }

        private void GoToOrderTray(Tray tray)
        {
            _currentState = ClientState.PickUpOrder;

            SetDestination(tray.transform.position, () =>
            {
                // Debug.Log("Дошенл до подноса ");
                GoToTableWithTray(tray);
            });
        }

        private void GoToTableWithTray(Tray tray)
        {
            if (_trayCoroutine != null)
                StopCoroutine(_trayCoroutine);

            _trayCoroutine = StartCoroutine(SmoothMoveTray(tray, () =>
            {
                _restaurant.RemoveClientTray(tray);
                _currentState = ClientState.Eat;

                SetDestination(Table.ClientSitPosition.transform.position, () =>
                {
                    _navMeshAgent.enabled = false;
                    transform.position = Table.ClientSitPosition.transform.position;
                    transform.rotation = Table.ClientSitPosition.transform.rotation;
                    _animator.SetBool("Sit", true);
                    Eat(tray);
                });
            }));
        }

        private IEnumerator SmoothMoveTray(Tray tray, System.Action onComplete)
        {
            float duration = 0.3f;
            float elapsedTime = 0f;

            Transform initialParent = tray.transform.parent;
            Vector3 initialPosition = tray.transform.position;
            Quaternion initialRotation = tray.transform.rotation;

            tray.transform.parent = this.transform;

            while (elapsedTime < duration)
            {
                tray.transform.position =
                    Vector3.Lerp(initialPosition, _trayPositionHand.position, elapsedTime / duration);
                tray.transform.rotation =
                    Quaternion.Lerp(initialRotation, _trayPositionHand.rotation, elapsedTime / duration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            tray.transform.position = _trayPositionHand.position;
            tray.transform.rotation = _trayPositionHand.rotation;

            onComplete?.Invoke();
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