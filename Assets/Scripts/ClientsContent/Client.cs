using System;
using System.Collections;
using Enums;
using OrdersContent;
using RestaurantContent;
using RestaurantContent.TableContent;
using RestaurantContent.TrayContent;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace ClientsContent
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        private Restaurant _restaurant;
        private Action<Client> _reachAction;
        private ClientState _currentState;

        public Order Order { get; private set; }

        public Table Table { get; private set; }

        /*private void Update()
        {
            if (_navMeshAgent.remainingDistance < 0.1f && _currentState == ClientState.PickUpOrder)
            {
                _currentState = ClientState.Eat;
                _navMeshAgent.SetDestination(Table.transform.position);
                Debug.Log("Клиент " + gameObject.name + " вернулся за стол.");
            }
        }*/
        
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

            SetDestination(position, (b) =>
            {
                Debug.Log("Встал в очередь " + gameObject.name);

                if (index == 0)
                {
                    Debug.Log("Первый");
                }
            });
        }

        public void OrderCompleted(Tray tray)
        {
            /*_navMeshAgent.SetDestination(position.position);
            _currentState = ClientState.PickUpOrder;*/
            
            StartCoroutine(PickUpOrder(tray));
        }
        
        private IEnumerator PickUpOrder(Tray tray)
        {
            /*Debug.Log("Текущая позиция агента: " + _navMeshAgent.transform.position);
            Debug.Log("Целевая позиция: " + tray.transform.position);*/
            
            _navMeshAgent.SetDestination(tray.transform.position);
            _currentState = ClientState.PickUpOrder;
            
            /*Debug.Log("Клиент идет за заказом " + _navMeshAgent.remainingDistance);
            Debug.Log("Клиент идет за заказом " + tray.name);
            Debug.Log("pathPending: " + _navMeshAgent.pathPending);
            Debug.Log("remainingDistance: " + _navMeshAgent.remainingDistance);
            Debug.Log("stoppingDistance: " + _navMeshAgent.stoppingDistance);*/
            
            
            while (_navMeshAgent.pathPending)
            {
                Debug.Log("Вычисляется путь...");
                yield return null;
            }

            // Ждем, пока клиент дойдет до подноса
            while (_navMeshAgent.remainingDistance > 0.1f)
            {
                // Debug.Log("Клиент " + gameObject.name + " идет к подносу. Осталось: " + _navMeshAgent.remainingDistance.ToString("F2") + " метров.");
                yield return null;
            }
     
            Debug.Log("Клиент дошел до заказом " + _navMeshAgent.remainingDistance);
            _restaurant.RemoveClientTray(tray);
            _currentState = ClientState.Eat;
            _navMeshAgent.SetDestination(Table.transform.position);
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
        }
        
        
        [ContextMenu("Completed")]
        public void Paid()
        {
            _currentState = ClientState.WaitingForOrder;
            _navMeshAgent.SetDestination(Table.ClientPosition.transform.position);
            Debug.Log("Клиент идет за стол " + _navMeshAgent.remainingDistance);
        }

        public void SetDestination(Vector3 position, Action<Client> reachAction)
        {
            /*this.goal = goal;
            anim.SetBool("Walking", true);

            obstacle.enabled = false;
            agent.enabled = true;*/
            _navMeshAgent.SetDestination(position);

            if (reachAction != null)
                _reachAction = reachAction;
        }

        public bool CanInteractWithCashier()
        {
            return _currentState == ClientState.AtCashier;
        }
    }
}