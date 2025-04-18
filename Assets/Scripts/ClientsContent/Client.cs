using OrdersContent;
using RestorantContent;
using UnityEngine;
using UnityEngine.AI;

namespace ClientsContent
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        
        private Order _order;
        private Transform _tablePosition;
        private Restaurant _restaurant;

        public void Init(Order order,Restaurant restaurant)
        {
            _order = order;
            _restaurant = restaurant;
        }

        public void GoToQueuePosition(Vector3 position)
        {
            _navMeshAgent.SetDestination(position);
        }

        public void Paid(Transform table)
        {
            
        }
    }
}