using System.Collections;
using QuestsContent.ProgressDailyTasksContent;
using UnityEngine;

namespace QuestsContent
{
    public class AttentionTaskActivator : MonoBehaviour
    {
        [SerializeField] private GameObject _attentionPrizeObject;
        [SerializeField] private ProgressDailyTasks _progressDailyTasks;
        [SerializeField] private DailyTasksCounter _dailyTasksCounter;
        [SerializeField] private TaskPrizeRecipient _prizeRecipient;
        [SerializeField] private ChainTasksCounter _chainTasksCounter;
        [SerializeField] private ChainTasksSaver _chainTasksSaver;

        private Coroutine _coroutine;

        private void OnEnable()
        {
            _progressDailyTasks.DailyProgressTasksChanged += Changed;
            _dailyTasksCounter.CurrentTasksChanged += Changed;
            _prizeRecipient.PrizeClaimed += ChangedDelay;
            _chainTasksSaver.ProgressChainTaskSaved += Changed;
            _chainTasksCounter.CurrentTaskChanged += Changed;
        }

        private void OnDisable()
        {
            _progressDailyTasks.DailyProgressTasksChanged -= Changed;
            _dailyTasksCounter.CurrentTasksChanged -= Changed;
            _chainTasksSaver.ProgressChainTaskSaved -= Changed;
            _chainTasksCounter.CurrentTaskChanged -= Changed;
        }

        private void Start()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartCheckPrize());
        }

        private void ChangedDelay()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartCheckPrize());
        }

        private IEnumerator StartCheckPrize()
        {
            yield return new WaitForSeconds(1f);
            Changed();
        }

        private void Changed()
        {
            if (GetCompleteDailyTasks() || GetProgressGlobalPrize() || GetProgressChainTask())
            {
                Debug.Log("Changed True");
                SetValue(true);
                return;
            }

            SetValue(false);
        }

        private void SetValue(bool value)
        {
            _attentionPrizeObject.SetActive(value);
        }

        private bool GetCompleteDailyTasks()
        {
            int value = 0;

            foreach (var task in _dailyTasksCounter.CurrentTasks)
            {
                if (task.IsCompleted && !task.IsReceived)
                    value++;
            }

            Debug.Log("CheckCompleteDailyTasks " + value);
            return value > 0;
        }

        private bool GetProgressGlobalPrize()
        {
            int value = 0;

            foreach (var task in _dailyTasksCounter.CurrentTasks)
            {
                if (task.IsCompleted)
                    value++;
            }

            Debug.Log("CheckProgressGlobalPrize " + value);
            return (value >= _dailyTasksCounter.CurrentTasks.Count && !_progressDailyTasks.IsReceived);
        }

        private bool GetProgressChainTask()
        {
            return (_chainTasksCounter.CurrentTask.IsCompleted && !_chainTasksCounter.CurrentTask.IsReceived);
        }
    }
}