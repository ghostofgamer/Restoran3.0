using System.Collections;
using UnityEngine;

namespace GiftContent
{
    public class Gift : MonoBehaviour
    {
        private float _duration = 6;
        private Coroutine _coroutine;
        private WaitForSeconds _waitForSeconds;

        private void Awake()
        {
            _waitForSeconds = new WaitForSeconds(_duration);
        }

        private void OnEnable()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartActive());
        }

        private IEnumerator StartActive()
        {
            yield return _waitForSeconds;
            gameObject.SetActive(false);
        }
    }
}