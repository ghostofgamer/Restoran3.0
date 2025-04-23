using System.Collections.Generic;
using UnityEngine;

namespace ClientsContent
{
    public class QueueCashRegister : MonoBehaviour
    {
        [SerializeField] public Transform[] _queuePositions;

        private Queue<Client> clientQueue = new Queue<Client>();
        private Client currentClient;
        private int _maxQueueSize = 5;

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
                    client.GoToQueuePosition(_queuePositions[index].position, index);
                    index++;
                }
            }
        }

        [ContextMenu("ClientFinishedOrder")]
        public void ClientFinishedOrder()
        {
            if (clientQueue.Count > 0)
            {
                Client client = clientQueue.Dequeue();
                UpdateQueuePositions();
            }
        }

        public bool IsQueueFull()
        {
            Debug.Log("IsQueueFull " + (clientQueue.Count >= _maxQueueSize));
            return clientQueue.Count >= _maxQueueSize;
        }
    }
}