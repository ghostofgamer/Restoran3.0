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
            Debug.Log("Колличество людей в очереди " +clientQueue.Count);
        }

        public void AddClientQueue(Client client)
        {
            clientQueue.Enqueue(client);
            Debug.Log("Колличество людей в очереди " +clientQueue.Count);
        }

        public void UpdateQueuePositions()
        {
            int index = 0;

            foreach (var client in clientQueue)
            {
                if (index < _queuePositions.Length)
                {
                    if (client.gameObject.activeSelf)
                    {
                        client.GoToQueuePosition(_queuePositions[index].position, index);
                    }

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
            Debug.Log("clientQueue.Count " + clientQueue.Count );
      
            return clientQueue.Count >= _maxQueueSize;
        }
    }
}