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

        private void OnEnable()
        {
            _tutorial.TutorCompleted += ShowStarterPack;
        }

        private void OnDisable()
        {
            _tutorial.TutorCompleted -= ShowStarterPack;
        }

        private void ShowStarterPack()
        {
            int value = PlayerPrefs.GetInt("StarterPack", 0);

            if (value > 0)
                return;

            _starterPackScreen.OpenScreen();
            _starterPackButton.SetActive(true);
        }
    }
}