using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CityTrafficContent
{
    public abstract class CityTraffic<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
        [SerializeField] private T[] _prefabs;
        [SerializeField] private Transform _container;
        [SerializeField] private int _spawnAmount;
        [SerializeField] protected int MaxActiveObject;

        private int _activeNPC = 0;

        protected List<ObjectPool<T>> _objectPools = new List<ObjectPool<T>>();

        private WaitForSeconds _waitOneSecond = new WaitForSeconds(1f);
        private WaitForSeconds _waitOtherSecondsSeconds = new WaitForSeconds(3f);

        private void Start()
        {
            foreach (var labubuNpcPrefab in _prefabs)
            {
                var clientPool = new ObjectPool<T>(labubuNpcPrefab, _spawnAmount, _container);
                clientPool.EnableAutoExpand();
                _objectPools.Add(clientPool);
            }

            StartCoroutine(SpawnNPC());
        }

        protected IEnumerator SpawnNPC()
        {
            while (true)
            {
                if (_activeNPC > MaxActiveObject)
                    yield return _waitOtherSecondsSeconds;
                else
                    yield return _waitOneSecond;

                int index = Random.Range(0, _spawnPoints.Count);
                SpawnRandomClient(_spawnPoints[index]);
            }
        }

        public void SpawnRandomClient(SpawnPoint spawnPoint)
        {
            ObjectPool<T> randomPool = _objectPools[Random.Range(0, _objectPools.Count)];
            T objectNpc = randomPool.GetFirstObject();

            if (objectNpc == null)
                return;

            SetPosition(objectNpc, spawnPoint);
            Init(objectNpc);
            objectNpc.gameObject.SetActive(true);
            IncreaseActiveNPC();
        }

        private void SetPosition(T objectNpc, SpawnPoint spawnPoint)
        {
            objectNpc.transform.position = spawnPoint.spawnPosition.position;
            /*objectNpc.Init(spawnPoint.pathGroups[Random.Range(0, spawnPoint.pathGroups.Count)], this,
                _textures[Random.Range(0, _textures.Length)]);*/
        }

        public abstract void Init(T g);


        public void IncreaseActiveNPC()
        {
            _activeNPC++;
        }

        public void DecreaseActiveNPC()
        {
            _activeNPC--;

            if (_activeNPC <= 0)
                _activeNPC = 0;
        }
    }

    [System.Serializable]
    public class SpawnPoint
    {
        public Transform spawnPosition;
        public List<GameObject> pathGroups;
    }
}