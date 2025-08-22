using System.Collections;
using I2.Loc;
using SettingsContent;
using UnityEngine;
using UnityEngine.UI;

namespace LoadingSceneContent
{
    public class LoadingGame : MonoBehaviour
    {
        [SerializeField] private GameObject _playButton;
        [SerializeField] private GameObject _sliderLoader;
        [SerializeField] private Image _loadingBar;
        [SerializeField] private float _loadingTime = 3f;
        [SerializeField] private LanguageChanger _languageChanger;
        
        private bool _isFirstTime;
        
        private void Start()
        {
            StartCoroutine(LoadAsync());
        }

        private IEnumerator LoadAsync()
        {
            float elapsedTime = 0f;
            float fillAmount = 0f;

            while (elapsedTime < _loadingTime)
            {
                elapsedTime += Time.deltaTime;
                fillAmount = Mathf.Clamp01(elapsedTime / _loadingTime);
                _loadingBar.fillAmount = fillAmount;
                yield return null;
            }

            _isFirstTime = PlayerPrefs.GetInt("IsFirstLanguageChange", 0) == 0;

            if (_isFirstTime)
            {
                string currentLanguage = LocalizationManager.GetCurrentDeviceLanguage();
                LocalizationManager.CurrentLanguage = currentLanguage;
                PlayerPrefs.SetInt("IsFirstLanguageChange", 1);
                _languageChanger.SetLanguageByName(currentLanguage);
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!! " + LocalizationManager.CurrentLanguage);
            }

            _playButton.SetActive(true);
            _sliderLoader.gameObject.SetActive(false);
        }
    }
}