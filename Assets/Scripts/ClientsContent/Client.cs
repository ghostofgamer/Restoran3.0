using System;
using Enums;
using OrdersContent;
using RestaurantContent;
using RestaurantContent.TableContent;
using UnityEngine;
using UnityEngine.AI;

namespace ClientsContent
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        private Order _order;
        private Table _table;
        private Restaurant _restaurant;
        private Action<Client> _reachAction;
        private ClientState _currentState;
        
        public void Init(Order order, Restaurant restaurant,Table table)
        {
            _order = order;
            _restaurant = restaurant;
            _currentState = ClientState.InQueue;
            _table = table;
        }

        public void GoToQueuePosition(Vector3 position, int index)
        {
            if(index==0)
                _currentState = ClientState.AtCashier;
            
            SetDestination(position,(b) =>
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
            _navMeshAgent.SetDestination(_table.transform.position);
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