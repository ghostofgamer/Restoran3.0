using System;
using System.Collections.Generic;
using System.IO;
using QuestsContent.ProgressDailyTasksContent;
using UnityEngine;

namespace QuestsContent
{
    public class DailyTasksSaver : MonoBehaviour
    {
        private const string SaveFileName = "daily_tasks_save.json";
        
        [SerializeField] private ProgressDailyTasks _progressDailyTasks;
        [SerializeField]private DailyTasksCounter _dailyTasksCounter;
        
        public void SaveProgress()
        {
            DailyTasksSaveData saveData = new DailyTasksSaveData
            {
                TasksData = new List<ChainTasksSaveData>(),
                IsReceivedGlobalDailyPrize = _progressDailyTasks.IsReceived
            };

            if (_dailyTasksCounter.CurrentTasks.Count != 4)
                return;

            foreach (Task task in _dailyTasksCounter.CurrentTasks)
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
                    TargetAmount = task.TargetAmount,
                    IsCompleted = task.IsCompleted,
                    IsReceived = task.IsReceived
                };
                saveData.TasksData.Add(taskData);
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
                    

                    return saveData;
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to load daily tasks progress: {e.Message}");
                    return null;
                }
            }
            
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
            }
            else
            {
                Debug.Log("No daily tasks save file found to delete.");
            }
        }
    
        [Serializable]
        public class DailyTasksSaveData
        {
            public List<ChainTasksSaveData> TasksData = new List<ChainTasksSaveData>();
            public bool IsReceivedGlobalDailyPrize;
        }
    }
}