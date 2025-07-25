using UnityEngine;

namespace QuestsContent.ProgressDailyTasksContent
{
    public class ProgressDailyTasks : MonoBehaviour
    {
        [SerializeField] private ProgressDailyTasksViewer _progressDailyTasksViewer;
        [SerializeField] private TasksActivator _tasksActivator;
        [SerializeField] private DailyTasksCounter _dailyTasksCounter;

        private int _currentValue;
        private int _targetValue;
        private bool _isReceived = false;

        private void OnEnable()
        {
            /*_tasksActivator.DailyTasksProgressChanged += ChangeValue;
            _tasksActivator.DailyTasksUpdated += ClearValue;*/
            _dailyTasksCounter.DailyTasksProgressChanged += ChangeValue;
            _dailyTasksCounter.DailyTasksUpdated += ClearValue;
        }

        private void OnDisable()
        {
            /*_tasksActivator.DailyTasksProgressChanged -= ChangeValue;
            _tasksActivator.DailyTasksUpdated -= ClearValue;*/
            _dailyTasksCounter.DailyTasksProgressChanged -= ChangeValue;
            _dailyTasksCounter.DailyTasksUpdated -= ClearValue;
        }

        public void GetPrize()
        {
            Debug.Log("GetPrize");
            SetReceivedValue(true);
            _tasksActivator.ChangeValue();
        }

        private void ChangeValue(int completedTasks, int maxTasks)
        {
            _progressDailyTasksViewer.ShowProgress(completedTasks, maxTasks, _isReceived);
        }

        private void ClearValue()
        {
            SetReceivedValue(false);
        }

        private void SetReceivedValue(bool value)
        {
            _isReceived = value;
        }
    }
}