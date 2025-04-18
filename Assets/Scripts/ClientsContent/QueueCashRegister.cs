using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ClientsContent
{
    public class QueueCashRegister : MonoBehaviour
    {
        [SerializeField] public Transform[] _queuePositions;
        
        private Queue<Client> clientQueue = new Queue<Client>();
        private Client currentClient;
        
        public void AddClientToQueue(Client client)
        {
            clientQueue.Enqueue(client);
            UpdateQueuePositions();
        }

        private void UpdateQueuePositions()
        {
            int index = 0;
            
            foreach (var client in clientQueue)
            {
                if (index < _queuePositions.Length)
                {
                    NavMeshAgent agent = client.GetComponent<NavMeshAgent>();
                    agent.SetDestination(_queuePositions[index].position);
                    index++;
                }
            }
        }
    }
}