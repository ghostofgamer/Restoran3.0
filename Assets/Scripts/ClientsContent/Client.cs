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
        [SerializeField] private Animator _animator;

        private Restaurant _restaurant;
        private Action<Client> _reachAction;
        private ClientState _currentState;
        private Coroutine _coroutine;

        public Order Order { get; private set; }

        public Table Table { get; private set; }

        private void Update()
        {
        }

        public void Init(Order order, Restaurant restaurant, Table table)
        {
            Order = order;
            _restaurant = restaurant;
            _currentState = ClientState.InQueue;
            Table = table;
            Order.SetTable(Table.Index);
        }

        public void GoToQueuePosition(Vector3 position, int index)
        {
            if (index == 0)
                _currentState = ClientState.AtCashier;

            SetDestination(position, () =>
            {
                Debug.Log("Встал в очередь " + gameObject.name);

                if (index == 0)
                {
                    Debug.Log("Первый");
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
            Debug.Log("GoToOrderTray");

            SetDestination(tray.transform.position, () =>
            {
                Debug.Log("Дошенл до подноса ");
                GoToTableWithTray(tray);
            });
        }

        private void GoToTableWithTray(Tray tray)
        {
            tray.transform.parent = this.transform;

            _restaurant.RemoveClientTray(tray);
            _currentState = ClientState.Eat;

            // _navMeshAgent.SetDestination(Table.ClientPosition.transform.position);

            SetDestination(Table.ClientPosition.transform.position, () =>
            {
                _navMeshAgent.enabled = false;
                transform.position = Table.ClientPosition.transform.position;
                transform.rotation = Table.ClientPosition.transform.rotation;
                _animator.SetBool("Sit", true);
                Debug.Log("Вернулся за стол с едой");
                Eat(tray);
            });
        }

        private void Eat(Tray tray)
        {
            Debug.Log("ЕМ");
            _currentState = ClientState.Eat;
            Debug.Log("Клиент " + gameObject.name + " вернулся за стол.");
            tray.SetActivity(false);
            tray.DefaultReturn();
        }


        /*private IEnumerator PickUpOrder(Tray tray)
        {
            // _navMeshAgent.SetDestination(tray.transform.position);

            SetDestination(tray.transform.position, () => { Debug.Log("Встал в очередь " + gameObject.name); });


            _currentState = ClientState.PickUpOrder;

            while (_navMeshAgent.pathPending)
            {
                Debug.Log("Вычисляется путь...");
                yield return null;
            }

            while (_navMeshAgent.remainingDistance > 0.1f)
            {
                // Debug.Log("Клиент " + gameObject.name + " идет к подносу. Осталось: " + _navMeshAgent.remainingDistance.ToString("F2") + " метров.");
                yield return null;
            }

            Debug.Log("Клиент дошел до заказом " + _navMeshAgent.remainingDistance);

            tray.transform.parent = this.transform;

            _restaurant.RemoveClientTray(tray);
            _currentState = ClientState.Eat;
            _navMeshAgent.SetDestination(Table.ClientPosition.transform.position);
            Debug.Log("Клиент идет за стол " + _navMeshAgent.remainingDistance);

            while (_navMeshAgent.pathPending)
            {
                // Debug.Log("Вычисляется путь...");
                yield return null;
            }

            while (_navMeshAgent.remainingDistance > 0.1f)
            {
                // Debug.Log("Клиент " + gameObject.name + " идет к столу. Осталось: " + _navMeshAgent.remainingDistance.ToString("F2") + " метров.");
                yield return null;
            }

            _currentState = ClientState.Eat;
            Debug.Log("Клиент " + gameObject.name + " вернулся за стол.");
            tray.SetActivity(false);
            tray.DefaultReturn();
        }*/


        [ContextMenu("Completed")]
        public void Paid()
        {
            StartCoroutine(StartPaid());

            /*_currentState = ClientState.WaitingForOrder;
            Debug.Log("Пошел ждать заказ");

            SetDestination(Table.ClientPosition.transform.position, () =>
            {
                Debug.Log("Жду за столом ");
            });*/
        }

        private IEnumerator StartPaid()
        {
            _animator.SetBool("Give", true);
            yield return new WaitForSeconds(1f);

            _currentState = ClientState.WaitingForOrder;
            Debug.Log("Пошел ждать заказ");
            _animator.SetBool("Give", false);

            SetDestination(Table.ClientPosition.transform.position, () => { Debug.Log("Жду за столом "); });
        }

        /*public void SetDestination(Vector3 position, Action<Client> reachAction)
        {
            /*this.goal = goal;
            anim.SetBool("Walking", true);

            obstacle.enabled = false;
            agent.enabled = true;#1#

            _navMeshAgent.SetDestination(position);
            _animator.SetBool("Walking",true);

            if (reachAction != null)
                _reachAction = reachAction;
        } */
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
            _navMeshAgent.SetDestination(position);
            _animator.SetBool("Walking", true);
            Debug.Log("Начал идти ");
            while (_navMeshAgent.pathPending)
            {
                yield return null;
            }

            while (_navMeshAgent.remainingDistance > 0.1f)
            {
                yield return null;
            }

            Debug.Log("Завершил идти ");
            _animator.SetBool("Walking", false);
            callback.Invoke();
        }


        public bool CanInteractWithCashier()
        {
            return _currentState == ClientState.AtCashier;
        }
    }
}