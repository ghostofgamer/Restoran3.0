using ClientsContent;
using UnityEngine;

namespace SpawnContent
{
    public class ClientsSpawner : MonoBehaviour
    {
        [SerializeField] private Client[] _clientPrefabs;
        [SerializeField] private Transform _container;
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private int _spawnAmount;

        private ObjectPool<Client> _clientPool;

        private void Start()
        {
            _clientPool = new ObjectPool<Client>(_clientPrefabs[0], _spawnAmount, _container);
            _clientPool.EnableAutoExpand();
        }

        public Client SpawnRandomClient()
        {
            Client randomClientPrefab = _clientPrefabs[Random.Range(0, _clientPrefabs.Length)];

            Client client = _clientPool.GetFirstObject();

            if (client == null)
            {
                client = Instantiate(randomClientPrefab, _container);
            }
            else
            {
                client.transform.position = _spawnPosition.position;
                client.transform.rotation = _spawnPosition.rotation;
                client.gameObject.SetActive(true);
            }

            return client;
        }
    }
}