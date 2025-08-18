using System.Collections;
using Enums;
using TutorialContent;
using UnityEngine;

namespace ADSContent.Popups
{
    public class PopUpAdActivator : MonoBehaviour
    {
        [SerializeField] private AddMoneyAdButton _popUpButton;
        [SerializeField] private float _duration;
        [SerializeField] private Tutorial _tutorial;

        private WaitForSeconds _waitForSeconds;
        private Coroutine _coroutine;

        private void OnEnable()
        {
            _tutorial.TutorCompleted += StartTiming;
        }

        private void OnDisable()
        {
            _tutorial.TutorCompleted -= StartTiming;
        }

        private void Start()
        {
            _waitForSeconds = new WaitForSeconds(_duration);

            if ((int)_tutorial.CurrentType < (int)TutorialType.TutorCompleted)
                return;

            StartTiming();
        }

        public void StartTiming()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ActivatePopUpButton());
        }

        private IEnumerator ActivatePopUpButton()
        {
            yield return _waitForSeconds;
            _popUpButton.Activate();
        }
    }
}