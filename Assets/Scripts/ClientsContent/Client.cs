using OrdersContent;
using UnityEngine;
using UnityEngine.AI;

namespace ClientsContent
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        private Order _order;

        public void Init(Order order)
        {
            _order = order;
        }

        public void GoToQueuePosition(Vector3 position)
        {
            _navMeshAgent.SetDestination(position);
        }
    }
}