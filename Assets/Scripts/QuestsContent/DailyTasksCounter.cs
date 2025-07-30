using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QuestsContent.ProgressDailyTasksContent;
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
        [SerializeField] private ProgressDailyTasks _progressDailyTasks;
        
        [SerializeField] private TMP_Text _taskSavedProgressText;
        [SerializeField] private TMP_Text _taskLoadedProgressText;

        private DateTime _lastGlobalUpdateTime;
        private DateTime _startTime;
        private const string LastGlobalUpdateTimeKey = "LastGlobalUpdateTime";
        private const int UpdateIntervalHours = 24;
        private const int UpdateIntervalSeconds = 24;
        private List<Task> _currentTasks = new List<Task>();

        private const string SaveFileName = "daily_tasks_save.json";

        public event Action<int, int> DailyTasksProgressChanged;
        public event Action DailyTasksUpdated;

        public void StartTasks()
        {
            foreach (var task in _dailyTasks)
                task.ResetTaskState();
            
            foreach (var task in _dailyTasks)
                task.UnsubscribeFromEvents();


            DailyTasksSaveData saveData = LoadProgress();

            if (saveData != null)
            {
                if (saveData.TasksData.Count > 0)
                {
                    if (saveData.TasksData.Count == _taskUIList.Count)
                    {
                        _currentTasks.Clear();

                        int index = 0;

                        foreach (var taskSaveData in saveData.TasksData)
                        {
                            Task taskByIndex = _dailyTasks.FirstOrDefault(task => task.TaskID == taskSaveData.TaskID);

                            if (taskByIndex != null)
                            {
                                taskByIndex.InitTaskUI(_taskUIList[index]);
                                taskByIndex.LoadProgress(taskSaveData.CurrentValue, taskSaveData.IsCompleted,
                                    taskSaveData.IsReceived);
                                _currentTasks.Add(taskByIndex);
                                index++;
                            }
                            else
                            {
                                Debug.LogError(
                                    $"No task with ID {taskSaveData.TaskID} found in the list of daily tasks.");
                            }
                        }
                    }
                    
                    Debug.Log("Loading Received Prize " + saveData.IsReceivedGlobalDailyPrize);

                    _progressDailyTasks.SetReceivedValue(saveData.IsReceivedGlobalDailyPrize);

                    foreach (var currentTask in _currentTasks)
                        currentTask.ProgressSaved += SaveProgress;

                    ChangeValue();
                }
            }
            else
            {
                AssignRandomTasksToUI();
            }


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
            foreach (var task in _dailyTasks)
                task.UnsubscribeFromEvents();
            
            foreach (var currentTask in _currentTasks)
                currentTask.ProgressSaved -= SaveProgress;
            
            foreach (var task in _dailyTasks)
                task.ResetTaskState();

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

            return value >= _currentTasks.Count;
        }

        private void AssignRandomTasksToUI()
        {
            foreach (var task in _dailyTasks)
                task.UnsubscribeFromEvents();
            
            foreach (var task in _dailyTasks)
                task.ResetTaskState();

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

            SaveProgress();

            foreach (var currentTask in _currentTasks)
                currentTask.ProgressSaved += SaveProgress;

            ChangeValue();
        }

        public void SaveProgress()
        {
            DailyTasksSaveData saveData = new DailyTasksSaveData
            {
                TasksData = new List<ChainTasksSaveData>(),
                IsReceivedGlobalDailyPrize = _progressDailyTasks.IsReceived
            };

            if (_currentTasks.Count != 4)
                return;

            foreach (Task task in _currentTasks)
            {
                if (task == null)
                {
                    Debug.LogWarning("Task is null, skipping save.");
                    continue;
                }

                ChainTasksSaveData taskData = new ChainTasksSaveData
                {
                    TaskIndex = task.Index,
                    TaskID = task.TaskID,
                    CurrentValue = task.CurrentValueTask,
                    IsCompleted = task.IsCompleted,
                    IsReceived = task.IsReceived
                };
                saveData.TasksData.Add(taskData);

                Debug.Log($"Saving Daily Task: TaskIndex={taskData.TaskIndex}, TaskID={taskData.TaskID}, " +
                          $"CurrentValue={taskData.CurrentValue}, IsCompleted={taskData.IsCompleted}, " +
                          $"IsReceived={taskData.IsReceived}");


                _taskSavedProgressText.text =
                    $"Saving Daily Task: TaskIndex={taskData.TaskIndex}, TaskID={taskData.TaskID}, " +
                    $"CurrentValue={taskData.CurrentValue}, IsCompleted={taskData.IsCompleted}, " +
                    $"IsReceived={taskData.IsReceived}";
            }

            try
            {
                string json = JsonUtility.ToJson(saveData, true);
                string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
                File.WriteAllText(filePath, json);
                Debug.Log($"Daily tasks progress saved to {filePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save daily tasks progress: {e.Message}");
            }
        }

        public DailyTasksSaveData LoadProgress()
        {
            string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);

            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    DailyTasksSaveData saveData = JsonUtility.FromJson<DailyTasksSaveData>(json);

                    _taskLoadedProgressText.text = $"Loading Daily Tasks: {saveData.TasksData.Count} tasks loaded.";
                    Debug.Log($"Loading Daily Tasks: {saveData.TasksData.Count} tasks,");

                    return saveData;
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to load daily tasks progress: {e.Message}");
                    return null;
                }
            }

            Debug.Log("No daily tasks save file found.");
            _taskLoadedProgressText.text = "No daily tasks progress data available.";
            return null;
        }

        [ContextMenu("Clear Daily Progress")]
        public void ClearProgress()
        {
            string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("Daily tasks progress file deleted successfully.");
                _taskSavedProgressText.text = "Daily tasks progress cleared.";
                _taskLoadedProgressText.text = "No daily tasks progress data available after clearing.";
            }
            else
            {
                Debug.Log("No daily tasks save file found to delete.");
                _taskSavedProgressText.text = "No daily tasks save file found to clear.";
                _taskLoadedProgressText.text = "No daily tasks progress data available.";
            }
        }
    }

    [Serializable]
    public class DailyTasksSaveData
    {
        public List<ChainTasksSaveData> TasksData = new List<ChainTasksSaveData>();
        public bool IsReceivedGlobalDailyPrize;
    }
}