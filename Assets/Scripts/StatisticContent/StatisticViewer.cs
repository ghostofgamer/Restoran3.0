using DayNightContent;
using UI.Screens;
using UnityEngine;

namespace StatisticContent
{
    public class StatisticViewer : MonoBehaviour
    {
        [SerializeField] private DayNightCycle _dayNightCycle;
        [SerializeField] private StatisticsScreen _statisticsScreen;

        private void OnEnable()
        {
            _dayNightCycle.DayOverCompleted += ShowStatistics;
        }

        private void OnDisable()
        {
            _dayNightCycle.DayOverCompleted -= ShowStatistics;
        }

        private void ShowStatistics()
        {
            _statisticsScreen.OpenScreen();
        }
    }
}