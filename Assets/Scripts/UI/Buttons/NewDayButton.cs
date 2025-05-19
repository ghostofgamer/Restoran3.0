using DayNightContent;
using SettingsContent.SoundContent;
using StatisticContent;
using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class NewDayButton : AbstractButton
    {
        [SerializeField] private DayNightCycle _dayNightCycle;
        [SerializeField] private StatisticsScreen _statisticsScreen;
        [SerializeField] private StatisticCounter _statisticCounter;
    
        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            _dayNightCycle.ResetDay();
            _statisticCounter.ClearData();
            _statisticsScreen.CloseScreen();
        }
    }
}