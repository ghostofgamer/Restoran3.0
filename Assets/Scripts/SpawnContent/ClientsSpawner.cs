using System.Collections;
using ClientsContent;
using UnityEngine;

namespace SpawnContent
{
    public class ClientsSpawner : MonoBehaviour
    {
        [SerializeField] private Client[] _clientPrefabs; 
        [SerializeField] private Transform _container;
        [SerializeField] private Transform _spawnPosition;
        
        private ObjectPool<Client> _clientPool;
        
        private void Start()
        {
            _clientPool = new ObjectPool<Client>(_clientPrefabs[0], 15, _container);
            _clientPool.EnableAutoExpand();
            StartCoroutine(StartSpawns());
        }

        private IEnumerator StartSpawns()
        {
            int value = 0;
            
            while (value <5)
            {
                yield return new WaitForSeconds(1f);
                value++;
                SpawnRandomClient();
                
            }
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