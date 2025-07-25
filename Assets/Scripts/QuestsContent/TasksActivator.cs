using System;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QuestsContent
{
    public class TasksActivator : MonoBehaviour
    {
        public static TasksActivator Instance { get; private set; }

        [SerializeField] private List<Task> _chainTasks = new List<Task>();
        [SerializeField] private List<Task> _dailyTasks = new List<Task>();
        [SerializeField] private List<TaskUI> _taskUIList = new List<TaskUI>();
        [SerializeField] private TaskUI _chainTaskUI;
        
        
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private bool _isTestMode = false;
        
        private DateTime _lastGlobalUpdateTime;
        private DateTime _startTime;
        private const string LastGlobalUpdateTimeKey = "LastGlobalUpdateTime";
        private const int UpdateIntervalHours = 24;
        private const int UpdateIntervalSeconds = 24;
        
        private int _currentTaskIndex = 0;
        private Task _currentTask;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            StartTask();
            
            AssignRandomTasksToUI();
            
            if (_isTestMode)
            {
                _startTime = DateTime.UtcNow;
            }
            else
            {
                LoadLastGlobalUpdateTime();
            }

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
            {
                timeUntilNextUpdate = _startTime.AddSeconds(UpdateIntervalSeconds) - DateTime.UtcNow;
            }
            else
            {
                timeUntilNextUpdate = _lastGlobalUpdateTime.AddHours(UpdateIntervalHours) - DateTime.UtcNow;
            }

            string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeUntilNextUpdate.Hours, timeUntilNextUpdate.Minutes, timeUntilNextUpdate.Seconds);
            _timeText.text = timeText;
        }

        [ContextMenu("Update Daily Tasks")]
        private void UpdateDailyTasks()
        {
            // Здесь вы можете добавить логику для обновления ежедневных задач
            Debug.Log("Daily tasks updated!");
            AssignRandomTasksToUI();
        }
        
        
        

        public void NextTask()
        {
            _currentTaskIndex++;
            Debug.Log("NextTask " + _currentTaskIndex);
            StartTask();
        }

        private void StartTask()
        {
            for (int i = 0; i < _chainTasks.Count; i++)
                _chainTasks[i].SetIndex(i);
            
            if (_currentTaskIndex >= _chainTasks.Count)
            {
                Debug.Log("All tasks completed!");
                return;
            }

            Task currentTask = _chainTasks[_currentTaskIndex];
            _currentTask = currentTask;
            currentTask.InitTaskUI(_chainTaskUI);
            currentTask.StartTask();
        }
        
        private void AssignRandomTasksToUI()
        {
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
                // taskUI.AssignTask(randomTask);
                tasksCopy.RemoveAt(randomIndex);
            }
        }
    }
}