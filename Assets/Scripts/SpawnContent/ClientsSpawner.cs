using System.Collections.Generic;
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
        private List<ObjectPool<Client>> _clientPools = new List<ObjectPool<Client>>();
        
        private void Start()
        {
            /*_clientPool = new ObjectPool<Client>(_clientPrefabs[0], _spawnAmount, _container);
            _clientPool.EnableAutoExpand();*/
            
            foreach (var clientPrefab in _clientPrefabs)
            {
                var clientPool = new ObjectPool<Client>(clientPrefab, _spawnAmount, _container);
                clientPool.EnableAutoExpand();
                _clientPools.Add(clientPool);
            }
        }
        
        public Client SpawnRandomClient()
        {
            ObjectPool<Client> randomPool = _clientPools[Random.Range(0, _clientPools.Count)];
            Client client = randomPool.GetFirstObject();

            if (client == null)
            {
                client = Instantiate(_clientPrefabs[0], _container);
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