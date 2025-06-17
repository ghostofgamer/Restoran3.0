using CalendarContent;
using I2.Loc;
using SettingsContent;
using TMPro;
using UnityEngine;

namespace UI.Screens
{
    public class EnergyGivingNewDayScreen : AbstractScreen
    {
        [SerializeField] private TMP_Text _dayValueText;
        [SerializeField] private LanguageChanger _languageChanger;
        [SerializeField] private Calendar _calendar;

        private void OnEnable()
        {
            _languageChanger.LanguageChanged += Show;
        }

        private void OnDisable()
        {
            _languageChanger.LanguageChanged -= Show;
        }

        public override void OpenScreen()
        {
            base.OpenScreen();
            Show();
        }

        private void Show()
        {
            _dayValueText.text =
                $"{LocalizationManager.GetTermTranslation("Daily Reward:")} {LocalizationManager.GetTermTranslation("Day")} {_calendar.CurrentDay}";
        }
    }
}