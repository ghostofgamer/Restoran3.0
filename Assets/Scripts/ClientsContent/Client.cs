using System;
using System.Collections;
using Enums;
using OrdersContent;
using RestaurantContent;
using RestaurantContent.TableContent;
using RestaurantContent.TrayContent;
using UnityEngine;
using UnityEngine.AI;

namespace ClientsContent
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private NavMeshObstacle _meshObstacle;
        [SerializeField] private Animator _animator;

        private Restaurant _restaurant;
        private Action<Client> _reachAction;
        private ClientState _currentState;
        private Coroutine _coroutine;
        private Transform _exitPosition;

        public Order Order { get; private set; }

        public Table Table { get; private set; }

        public void Init(Order order, Restaurant restaurant, Table table, Transform exitPosition)
        {
            Order = order;
            _restaurant = restaurant;
            _currentState = ClientState.InQueue;
            Table = table;
            Order.SetTable(Table.Index);
            _exitPosition = exitPosition;
        }

        public void GoToQueuePosition(Vector3 position, int index)
        {
            if (index == 0)
                _currentState = ClientState.AtCashier;

            SetDestination(position, () =>
            {
                // Debug.Log("Встал в очередь " + gameObject.name);

                if (index == 0)
                {
                    // Debug.Log("Первый");
                    // Дополнительные действия, если клиент первый в очереди
                }
            });
        }

        public void OrderCompleted(Tray tray)
        {
            // StartCoroutine(PickUpOrder(tray));
            GoToOrderTray(tray);
        }

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
            Table.SetBusyValue(false);
            _animator.SetBool("Sit", false);

            SetDestination(_exitPosition.transform.position, () => { gameObject.SetActive(false); });
        }

        [ContextMenu("Completed")]
        public void Paid()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartPaid());
        }

        private IEnumerator StartPaid()
        {
            _animator.SetBool("Give", true);
            yield return new WaitForSeconds(1f);

            _currentState = ClientState.WaitingForOrder;
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
            /*this.goal = goal;
            anim.SetBool("Walking", true);

            obstacle.enabled = false;
            agent.enabled = true;*/

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(MoveToPosition(position, callback));
        }

        private IEnumerator MoveToPosition(Vector3 position, System.Action callback)
        {
            /*Debug.Log("_navMeshAgent.enabled " + _navMeshAgent.enabled);
            Debug.Log("_currentState " + _currentState);*/
            _meshObstacle.enabled = false;
            
            if (!_navMeshAgent.enabled)
            {
                _navMeshAgent.enabled = true;
                transform.position = Table.ClientStandPosition.position;
                transform.rotation = Table.ClientStandPosition.rotation;
            }

            if (_animator.GetBool("Sit"))
            {
                _animator.SetBool("Sit", false);
            }

            _navMeshAgent.SetDestination(position);
            _animator.SetBool("Walking", true);
            // Debug.Log("Начал идти ");
            while (_navMeshAgent.pathPending)
            {
                yield return null;
            }

            while (_navMeshAgent.remainingDistance > 0.1f)
            {
                yield return null;
            }
            
            _meshObstacle.enabled = true;
            // Debug.Log("Завершил идти ");
            _animator.SetBool("Walking", false);
            callback.Invoke();
        }


        public bool CanInteractWithCashier()
        {
            return _currentState == ClientState.AtCashier;
        }
    }
}