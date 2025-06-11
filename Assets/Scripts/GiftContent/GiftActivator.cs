using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GiftContent
{
    public class GiftActivator : MonoBehaviour
    {
        [SerializeField] private List<Transform> _positions;
        [SerializeField] private Gift[] _gifts;

        private List<ObjectPool<Gift>> _giftPools = new List<ObjectPool<Gift>>();
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(10f);
        private List<GameObject> _allObjects = new List<GameObject>();

        private void Start()
        {
            foreach (var gift in _gifts)
            {
                var clientPool = new ObjectPool<Gift>(gift, 5, transform);
                clientPool.EnableAutoExpand();
                _giftPools.Add(clientPool);
            }

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ActivateObjects());
        }

        private IEnumerator ActivateObjects()
        {
            while (true)
            {
                yield return _waitForSeconds;

                if (_positions.Count >= 2 && _gifts.Length >= 2)
                {
                    int posIndex1 = Random.Range(0, _positions.Count);
                    int posIndex2 = Random.Range(0, _positions.Count);

                    while (posIndex2 == posIndex1)
                        posIndex2 = Random.Range(0, _positions.Count);

                    InitPosition(_positions[posIndex1]);
                    InitPosition(_positions[posIndex2]);
                }
                else
                {
                    Debug.LogWarning("Not enough positions or objects to activate two objects.");
                }
            }
        }

        private void InitPosition(Transform transform)
        {
            ObjectPool<Gift> randomPool = _giftPools[Random.Range(0, _giftPools.Count)];
            Gift gift = randomPool.GetFirstObject();

            if (gift != null)
            {
                gift.transform.position = transform.position;
                gift.transform.rotation = transform.rotation;
                gift.gameObject.SetActive(true);
            }
        }
    }
}