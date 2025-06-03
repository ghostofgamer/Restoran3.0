using System.Collections;
using Enums;
using TutorialContent;
using UI.Screens.AdsScreens;
using UnityEngine;

namespace ADSContent.Popups
{
    public class AdPopupActivator : MonoBehaviour
    {
        [SerializeField] private StarterPackScreen _starterPackScreen;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private GameObject _starterPackButton;

        private Coroutine _starterPackCoroutine;
        private WaitForSeconds _waitForSecondsStarterPack = new WaitForSeconds(6f);

        private void OnEnable()
        {
            _tutorial.TutorCompleted += ShowStarterPack;
        }

        private void OnDisable()
        {
            _tutorial.TutorCompleted -= ShowStarterPack;
        }

        private void Start()
        {
            if ((int)_tutorial.CurrentType >= (int)TutorialType.TutorCompleted)
                ShowStarterPack();
        }

        private void ShowStarterPack()
        {
            int value = PlayerPrefs.GetInt("StarterPack", 0);

            if (value > 0)
                return;
            
            if (_starterPackCoroutine != null)
                StopCoroutine(_starterPackCoroutine);

            _starterPackCoroutine = StartCoroutine(StarterPack());
        }

        private IEnumerator StarterPack()
        {
            yield return _waitForSecondsStarterPack;
            _starterPackScreen.OpenScreen();
            _starterPackButton.SetActive(true);
        }
    }
}