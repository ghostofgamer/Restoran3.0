using TMPro;
using UnityEngine;

namespace CalendarContent
{
    public class CalendarViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _dayText;
        [SerializeField] private Calendar _calendar;

        private void OnEnable()
        {
            _calendar.DayChanged += Show;
        }

        private void OnDisable()
        {
            _calendar.DayChanged -= Show;
        }

        private void Show()
        {
            _dayText.text = $"DAY {_calendar.CurrentDay}";
        }
    }
}