using System;
using System.Collections.Generic;
using System.Linq;
using QuestsContent;
using QuestsContent.ProgressDailyTasksContent;
using TMPro;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QuestsContent
{
    public class DailyTasksCounter : MonoBehaviour
    {
        private const string CurrentTasksKey = "CurrentTasks";
        private const string LastGlobalUpdateTimeKey = "LastGlobalUpdateTime";

        [SerializeField] private List<Task> _dailyTasks = new List<Task>();
        [SerializeField] private List<TaskUI> _taskUIList = new List<TaskUI>();
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private bool _isTestMode = false;
        [SerializeField] private ProgressDailyTasks _progressDailyTasks;

        private DateTime _lastGlobalUpdateTime;
        private DateTime _startTime;
        private const int UpdateIntervalHours = 24;
        private const int UpdateIntervalSeconds = 24;
        private List<Task> _currentTasks = new List<Task>();

        public event Action<int, int> DailyTasksProgressChanged;
        public event Action DailyTasksUpdated;

        public void StartTasks()
        {
            AssignRandomTasksToUI();
            
            /*if (!LoadSavedTasks())
            {
                AssignRandomTasksToUI();
            }*/

            if (_isTestMode)
                _startTime = DateTime.UtcNow;
            else
                LoadLastGlobalUpdateTime();

            UpdateTimeText();
        }

        /*
        private void OnApplicationQuit()
        {
            SaveCurrentTasks();
        }
        */

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
            Debug.Log("Daily tasks updated!");
            DailyTasksUpdated?.Invoke();
            AssignRandomTasksToUI();
            SaveCurrentTasks();
        }

        public void ChangeValue()
        {
            int value = 0;

            foreach (Task task in _currentTasks)
            {
                if (task.IsCompleted)
                    value++;
            }

            Debug.Log("---------------------------ChangeValueTask " + value);
            DailyTasksProgressChanged?.Invoke(value, _currentTasks.Count);
        }

        public bool CheckCompletion()
        {
            int value = 0;

            foreach (Task task in _currentTasks)
            {
                if (task.IsCompleted)
                    value++;
            }

            return value >= _currentTasks.Count;
        }

        private void AssignRandomTasksToUI()
        {
            ClearAllDatas();
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

            SaveCurrentTasks();
            ChangeValue();
        }

        [ContextMenu("Save Current Tasks")]
        public void SaveCurrentTasks()
        {
            TaskDataWrapper wrapper = new TaskDataWrapper
            {
                tasks = _currentTasks.Select(task => new TaskData
                {
                    taskId = task.TaskId,
                    isCompleted = task.IsCompleted,
                    isReceived = task.IsReceived,
                    currentValue = task.CurrentValue
                }).ToArray()
            };

            string tasksJson = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString(CurrentTasksKey, tasksJson);
            PlayerPrefs.Save();
            Debug.Log("Tasks saved: " + tasksJson);
        }

        private bool LoadSavedTasks()
        {
            if (!PlayerPrefs.HasKey(CurrentTasksKey))
            {
                Debug.Log("No saved tasks found.");
                return false;
            }

            string tasksJson = PlayerPrefs.GetString(CurrentTasksKey);
            Debug.Log("tasksJson " + tasksJson);
            TaskDataWrapper wrapper = JsonUtility.FromJson<TaskDataWrapper>(tasksJson);

            if (wrapper == null || wrapper.tasks == null || wrapper.tasks.Length == 0)
            {
                Debug.LogWarning("Invalid or empty saved tasks data.");
                return false;
            }

            _currentTasks.Clear();
            int uiIndex = 0;

            foreach (TaskData taskData in wrapper.tasks)
            {
                Task task = _dailyTasks.Find(t => t.TaskId == taskData.taskId);

                if (task != null && uiIndex < _taskUIList.Count)
                {
                    // Debug.Log("-----------------TaskDataLoadInfo " + taskData.currentValue + " " + taskData.taskId);
                    
                    task.InitTaskUI(_taskUIList[uiIndex]);
                    _currentTasks.Add(task);
                    task.LoadProgress(JsonUtility.ToJson(taskData));
                    task.VirtualShowProgress();
                    Debug.Log("============================TaskComplited " + task.IsCompleted );

                    /*task.InitTaskUI(_taskUIList[uiIndex]);
                    task.LoadProgress(JsonUtility.ToJson(taskData));
                    task.VirtualShowProgress();
                    task.StartTask();
                    _currentTasks.Add(task);
                    task.LoadGameProgress(task.CurrentValue);*/

                    uiIndex++;
                }
                else
                {
                    Debug.LogWarning($"Task with ID {taskData.taskId} not found or not enough TaskUI elements.");
                }
            }

            if (_currentTasks.Count > 0)
            {
                _progressDailyTasks.LoadData();
                
                ChangeValue();
                return true;
            }

            Debug.Log("No valid tasks loaded.");
            return false;
        }

        [ContextMenu("Clear All Datas")]
        public void ClearAllDatas()
        {
            foreach (var dailyTask in _dailyTasks)
                dailyTask.ClearProgress();
        }
    }
}

[System.Serializable]
public class TaskIdsWrapper
{
    public string[] taskIds;
}

[System.Serializable]
public class TaskDataWrapper
{
    public TaskData[] tasks;
}