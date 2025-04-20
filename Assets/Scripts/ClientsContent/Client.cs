using System;
using Enums;
using OrdersContent;
using RestaurantContent;
using RestaurantContent.TableContent;
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

        [ContextMenu("Completed")]
        public void Paid()
        {
            _currentState = ClientState.WaitingForOrder;
            _navMeshAgent.SetDestination(Table.transform.position);
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