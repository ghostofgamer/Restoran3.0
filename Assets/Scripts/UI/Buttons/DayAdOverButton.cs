using ADSContent;
using DayNightContent;
using SettingsContent.SoundContent;
using StatisticContent;
using UI.Screens;
using UnityEngine;
using WalletContent;

namespace UI.Buttons
{
    public class DayAdOverButton : AbstractButton
    {
        [SerializeField] private ADS _ads;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private DayNightCycle _dayNightCycle;
        [SerializeField] private StatisticsScreen _statisticsScreen;
        [SerializeField] private StatisticCounter _statisticCounter;

        public override void OnClick()
        {
            SoundPlayer.Instance.PlayButtonClick();
            
            _ads.ShowRewarded(() =>
            {
                _wallet.Add(new DollarValue(25, 0));
                _dayNightCycle.ResetDay();
                _statisticCounter.ClearData();
                _statisticsScreen.CloseScreen();
            });
        }
    }
}