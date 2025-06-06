using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

namespace SettingsContent
{
    public class LanguageChanger : MonoBehaviour
    {
        private List<string> _languages = new List<string>
        {
            "English", "Spanish", "French", "German", "Italian", "Russian", "Japanese", "Turkish", "Polish",
            "Portuguese", "Indonesian"
        };

        private int currentIndex = 0;

        private void Start()
        {
            if (PlayerPrefs.HasKey("LanguageIndex"))
                currentIndex = PlayerPrefs.GetInt("LanguageIndex");

            UpdateLanguageText();
        }

        public void PrevLanguage()
        {
            currentIndex = (currentIndex - 1 + _languages.Count) % _languages.Count;
            UpdateLanguageText();
            SaveIndex();
        }

        public void NextLanguage()
        {
            currentIndex = (currentIndex + 1) % _languages.Count;
            UpdateLanguageText();
            SaveIndex();
        }

        private void UpdateLanguageText()
        {
            if (LocalizationManager.HasLanguage(_languages[currentIndex]))
                LocalizationManager.CurrentLanguage = _languages[currentIndex];
        }

        private void SaveIndex()
        {
            PlayerPrefs.SetInt("LanguageIndex", currentIndex);
            PlayerPrefs.Save();
        }
    }
}