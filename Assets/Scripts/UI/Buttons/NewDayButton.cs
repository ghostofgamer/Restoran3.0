using DayNightContent;
using UI.Screens;
using UnityEngine;

namespace UI.Buttons
{
    public class NewDayButton : AbstractButton
    {
        [SerializeField] private DayNightCycle _dayNightCycle;
        [SerializeField] private StatisticsScreen _statisticsScreen;
    
        public override void OnClick()
        {
            Debug.Log("новый");
            _dayNightCycle.ResetDay();
            _statisticsScreen.CloseScreen();
        }
    }
}