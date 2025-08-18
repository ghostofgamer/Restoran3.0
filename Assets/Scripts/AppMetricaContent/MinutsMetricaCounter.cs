using System.Collections;
using Io.AppMetrica;
using UnityEngine;

namespace AppMetricaContent
{
    public class MinutesMetricaCounter : MonoBehaviour
    {
        private int _minutesPlay = 0;
        private Coroutine _coroutine;
        private int _reportInterval = 1;
        private int _lastReportMinute = 0;

        private void Start()
        {
            _minutesPlay = PlayerPrefs.GetInt("MinutesPlay", 0);
            _lastReportMinute = PlayerPrefs.GetInt("LastReportMinute", 0);

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartTimer());
        }

        private IEnumerator StartTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(60f);
                _minutesPlay++;
                _reportInterval = GetReportInterval(_minutesPlay);
                PlayerPrefs.SetInt("MinutesPlay", _minutesPlay);

                if (_minutesPlay - _lastReportMinute >= _reportInterval)
                {
                    SendReport();
                    _lastReportMinute = _minutesPlay;
                    PlayerPrefs.SetInt("LastReportMinute", _lastReportMinute);
                }
            }
        }

        private int GetReportInterval(int minutes)
        {
            if (minutes < 60) return 1; // Первый час: каждую минуту
            if (minutes < 600) return 5; // С 1 до 10 часов: каждые 5 минут
            return 10; // После 10 часов: каждые 10 минут
        }

        private void SendReport()
        {
            AppMetrica.ReportEvent("Minutes", "{\"" + _minutesPlay + "\":null}");
        }
    }
}