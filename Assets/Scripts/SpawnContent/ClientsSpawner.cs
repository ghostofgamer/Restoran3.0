using ClientsContent;
using UnityEngine;

namespace SpawnContent
{
    public class ClientsSpawner : MonoBehaviour
    {
        [SerializeField] private Client[] _clientPrefabs; 
        [SerializeField] private Transform _container;
        
        private ObjectPool<Client> _clientPool;
        
        private void Start()
        {
            _clientPool = new ObjectPool<Client>(_clientPrefabs[0], 15, _container);
            _clientPool.EnableAutoExpand();
        }
        
        public Client SpawnRandomClient()
        {
            // Выбираем случайный префаб клиента
            Client randomClientPrefab = _clientPrefabs[Random.Range(0, _clientPrefabs.Length)];

            // Получаем объект из пула или создаем новый, если пул пуст
            Client client = _clientPool.GetFirstObject();

            if (client == null)
            {
                // Если пул пуст, создаем новый объект
                client = Instantiate(randomClientPrefab, _container);
            }
            else
            {
                // Обновляем объект из пула
                client.transform.position = _container.position;
                client.transform.rotation = _container.rotation;
                client.gameObject.SetActive(true);
            }

            return client;
        }
    }
}