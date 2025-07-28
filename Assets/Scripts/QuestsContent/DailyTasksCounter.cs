using System;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QuestsContent
{
    public class DailyTasksCounter : MonoBehaviour
    {
        [SerializeField] private List<Task> _dailyTasks = new List<Task>();
        [SerializeField] private List<TaskUI> _taskUIList = new List<TaskUI>();
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private bool _isTestMode = false;

        private DateTime _lastGlobalUpdateTime;
        private DateTime _startTime;
        private const string LastGlobalUpdateTimeKey = "LastGlobalUpdateTime";
        private const int UpdateIntervalHours = 24;
        private const int UpdateIntervalSeconds = 24;
        private List<Task> _currentTasks = new List<Task>();

        public event Action<int, int> DailyTasksProgressChanged;
        public event Action DailyTasksUpdated;

        public void StartTasks()
        {
            AssignRandomTasksToUI();

            if (_isTestMode)
                _startTime = DateTime.UtcNow;
            else
                LoadLastGlobalUpdateTime();

            UpdateTimeText();
        }

        private void Update()
        {
            UpdateTimeText();
        }

        private void LoadLastGlobalUpdateTime()
        {
            if (PlayerPrefs.HasKey(LastGlobalUpdateTimeKey))
            {
                string savedTime = PlayerPrefs.GetString(LastGlobalUpdateTimeKey);
                _lastGlobalUpdateTime = DateTime.Parse(savedTime);
            }
            else
            {
                // Устанавливаем последнее глобальное время обновления на полночь по UTC
                _lastGlobalUpdateTime = DateTime.UtcNow.Date;
                SaveLastGlobalUpdateTime();
            }
        }

        private void SaveLastGlobalUpdateTime()
        {
            PlayerPrefs.SetString(LastGlobalUpdateTimeKey, _lastGlobalUpdateTime.ToString());
            PlayerPrefs.Save();
        }

        private void UpdateTimeText()
        {
            TimeSpan timeSinceLastUpdate;

            if (_isTestMode)
            {
                timeSinceLastUpdate = DateTime.UtcNow - _startTime;

                if (timeSinceLastUpdate.TotalSeconds >= UpdateIntervalSeconds)
                {
                    _startTime = DateTime.UtcNow;
                    UpdateDailyTasks();
                }
            }
            else
            {
                timeSinceLastUpdate = DateTime.UtcNow - _lastGlobalUpdateTime;

                if (timeSinceLastUpdate.TotalHours >= UpdateIntervalHours)
                {
                    _lastGlobalUpdateTime = DateTime.UtcNow.Date;
                    SaveLastGlobalUpdateTime();
                    UpdateDailyTasks();
                }
            }

            TimeSpan timeUntilNextUpdate;

            if (_isTestMode)
                timeUntilNextUpdate = _startTime.AddSeconds(UpdateIntervalSeconds) - DateTime.UtcNow;
            else
                timeUntilNextUpdate = _lastGlobalUpdateTime.AddHours(UpdateIntervalHours) - DateTime.UtcNow;

            string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeUntilNextUpdate.Hours,
                timeUntilNextUpdate.Minutes, timeUntilNextUpdate.Seconds);
            _timeText.text = timeText;
        }

        [ContextMenu("Update Daily Tasks")]
        private void UpdateDailyTasks()
        {
            // Здесь вы можете добавить логику для обновления ежедневных задач
            Debug.Log("Daily tasks updated!");
            DailyTasksUpdated?.Invoke();
            AssignRandomTasksToUI();
        }

        public void ChangeValue()
        {
            int value = 0;

            foreach (Task task in _currentTasks)
            {
                if (task.IsCompleted)
                    value++;
            }

            DailyTasksProgressChanged?.Invoke(value, _currentTasks.Count);
            Debug.Log("Value " + value);
        }

        public bool CheckCompletion()
        {
            int value = 0;

            foreach (Task task in _currentTasks)
            {
                if (task.IsCompleted)
                    value++;
            }
            
            return value>= _currentTasks.Count;
        }

        private void AssignRandomTasksToUI()
        {
            _currentTasks.Clear();
            List<Task> tasksCopy = new List<Task>(_dailyTasks);

            foreach (TaskUI taskUI in _taskUIList)
            {
                if (tasksCopy.Count == 0)
                {
                    Debug.LogWarning("Not enough tasks for TaskUI");
                    break;
                }

                int randomIndex = Random.Range(0, tasksCopy.Count);
                Task randomTask = tasksCopy[randomIndex];
                randomTask.InitTaskUI(taskUI);
                randomTask.StartTask();
                _currentTasks.Add(randomTask);
                // taskUI.AssignTask(randomTask);
                tasksCopy.RemoveAt(randomIndex);
            }

            ChangeValue();
        }
    }
}