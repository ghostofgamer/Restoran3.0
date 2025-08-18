using I2.Loc;
using UnityEngine;

namespace UI.Screens
{
    public class FirstLanguageScreen : AbstractScreen
    {
        private bool _isFirstTime;

        private void Start()
        {
            _isFirstTime = PlayerPrefs.GetInt("IsFirstLanguageChange", 0) == 0;

            if (_isFirstTime)
            {
                OpenScreen();
                string currentLanguage = LocalizationManager.GetCurrentDeviceLanguage();
                LocalizationManager.CurrentLanguage = currentLanguage;
                PlayerPrefs.SetInt("IsFirstLanguageChange", 1);
            }
            else
                CloseScreen();
        }
    }
}