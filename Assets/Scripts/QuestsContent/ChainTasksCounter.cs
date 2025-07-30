using System.Collections.Generic;
using System.IO;
using TMPro;
using UI;
using UnityEngine;

namespace QuestsContent
{
    public class ChainTasksCounter : MonoBehaviour
    {
        [SerializeField] private List<Task> _chainTasks = new List<Task>();
        [SerializeField] private TaskUI _chainTaskUI;
        [SerializeField] private GameObject _taskLock;

        [SerializeField] private TMP_Text _saveValueText;
        [SerializeField] private TMP_Text _loadValueText;

        private int _currentTaskIndex = 0;
        private Task _currentTask;

        private const string SaveFileName = "chain_tasks_save.json";

        public void NextTask()
        {
            _currentTask.ProgressSaved -= SaveProgress;
            Debug.Log("_currentTaskIndex " + _currentTaskIndex);
            _currentTaskIndex++;
            PlayerPrefs.SetInt("CurrentChainTaskIndex", _currentTaskIndex);
            Debug.Log("NextTask " + _currentTaskIndex);
            
            if (CheckLockChainTasksValue())
                return;
            
            StartNextTask();
            // StartTask();
        }

        public void StartTask()
        {
            for (int i = 0; i < _chainTasks.Count; i++)
                _chainTasks[i].SetIndex(i);

            _currentTaskIndex = PlayerPrefs.GetInt("CurrentChainTaskIndex", _currentTaskIndex);
            
            if (CheckLockChainTasksValue())
                return;

            ChainTasksSaveData saveData = LoadProgress();

            if (saveData == null|| saveData.TaskIndex != _currentTaskIndex)
            {
                Debug.Log("No progress data found. Starting from the beginning.");
                
                Task currentTask = _chainTasks[_currentTaskIndex];
                _currentTask = currentTask;
                _currentTask.ProgressSaved += SaveProgress;
                Debug.Log("!!!_currentTask " + _currentTask.Index);
                currentTask.InitTaskUI(_chainTaskUI);
                currentTask.StartTask();
            }
            else
            {
                Debug.Log("!!!_currentTask LoadSaveData " + saveData.TaskIndex);

                Task currentTask = _chainTasks[_currentTaskIndex];
                _currentTask = currentTask;
                _currentTask.ProgressSaved += SaveProgress;
                _currentTask.InitTaskUI(_chainTaskUI);
                _currentTask.LoadProgress(saveData.CurrentValue, saveData.IsCompleted, saveData.IsReceived);
            }
        }

        public void StartNextTask()
        {
            Debug.Log("No progress data found. Starting from the beginning.");
            Task currentTask = _chainTasks[_currentTaskIndex];
            _currentTask = currentTask;
            _currentTask.ProgressSaved += SaveProgress;
            Debug.Log("!!!_currentTask " + _currentTask.Index);
            currentTask.InitTaskUI(_chainTaskUI);
            currentTask.StartTask();
        }

        public void SaveProgress()
        {
            if (_currentTask == null)
            {
                Debug.LogWarning("No current task to save.");
                return;
            }

            ChainTasksSaveData saveData = new ChainTasksSaveData
            {
                TaskIndex = _currentTask.Index,
                TaskID = _currentTask.TaskID,
                CurrentValue = _currentTask.CurrentValueTask,
                IsCompleted = _currentTask.IsCompleted,
                IsReceived = _currentTask.IsReceived
            };

            Debug.Log($"Saving Chain progress: TaskIndex={saveData.TaskIndex}, TaskID={saveData.TaskID}, " +
                      $"CurrentValue={saveData.CurrentValue}, IsCompleted={saveData.IsCompleted}, " +
                      $"IsReceived={saveData.IsReceived}");

            _saveValueText.text = $"Saving Chain progress: TaskIndex={saveData.TaskIndex}, TaskID={saveData.TaskID}, " +
                                  $"CurrentValue={saveData.CurrentValue}, IsCompleted={saveData.IsCompleted}, " +
                                  $"IsReceived={saveData.IsReceived}";
            try
            {
                string json = JsonUtility.ToJson(saveData, true);
                string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
                File.WriteAllText(filePath, json);
                Debug.Log($"Progress saved to {filePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save progress: {e.Message}");
            }
        }

        public ChainTasksSaveData LoadProgress()
        {
            string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                ChainTasksSaveData saveData = JsonUtility.FromJson<ChainTasksSaveData>(json);

                // Логируем загруженные данные для отладки
                Debug.Log($"Loading Chain progress: TaskIndex={saveData.TaskIndex}, TaskID={saveData.TaskID}, " +
                          $"CurrentValue={saveData.CurrentValue}, IsCompleted={saveData.IsCompleted}, " +
                          $"IsReceived={saveData.IsReceived}");

                _loadValueText.text =
                    $"Loading Chain progress: TaskIndex={saveData.TaskIndex}, TaskID={saveData.TaskID}, " +
                    $"CurrentValue={saveData.CurrentValue}, IsCompleted={saveData.IsCompleted}, " +
                    $"IsReceived={saveData.IsReceived}";

                return saveData;
            }

            return null;
        }

        private bool CheckLockChainTasksValue()
        {
            if (_currentTaskIndex >= _chainTasks.Count)
            {
                Debug.Log("All tasks completed!");
                _taskLock.SetActive(true);
                return true;
            }
            
            _taskLock.SetActive(false);
            return false;
        }


        [ContextMenu("Clear Progress")]
        public void ClearProgress()
        {
            string filePath = Path.Combine(Application.persistentDataPath, SaveFileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("Saved progress file deleted successfully.");
                _saveValueText.text = "Saved progress cleared.";
                _loadValueText.text = "No progress data available after clearing.";
            }
        }
    }

    [System.Serializable]
    public class ChainTasksSaveData
    {
        public int TaskIndex;
        public string TaskID;
        public int CurrentValue;
        public bool IsCompleted;
        public bool IsReceived;
    }
}