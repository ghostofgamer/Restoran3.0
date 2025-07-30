using ADSContent;
using Io.AppMetrica;
using QuestsContent;
using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class DailyGlobalPrizeButton : AbstractButton
    {
        [SerializeField] private ADS _ads;
        [SerializeField] private bool _isADS;
        [SerializeField] private TaskPrizeRecipient _taskPrizeRecipient;
        [SerializeField] private DailyGlobalTaskPrizeScreen _dailyGlobalTaskPrizeScreen;
        [SerializeField] private TasksScreen _tasksScreen;
        
        public override void OnClick()
        {
            if (_isADS)
            {
                _ads.ShowRewarded(() =>
                {
                    AppMetrica.ReportEvent("RewardAD", "{\"" + "Daily_Prize_x2_ADS" + "\":null}");
                    _taskPrizeRecipient.ClaimGlobalDailyPrize(_dailyGlobalTaskPrizeScreen.RandomPrizes, true);
                    _dailyGlobalTaskPrizeScreen.CloseScreen();
                    _tasksScreen.CloseScreen();
                });
            }
            else
            {
                _taskPrizeRecipient.ClaimGlobalDailyPrize(_dailyGlobalTaskPrizeScreen.RandomPrizes, false);
                _dailyGlobalTaskPrizeScreen.CloseScreen();
                _tasksScreen.CloseScreen();
            }
        }
    }
}