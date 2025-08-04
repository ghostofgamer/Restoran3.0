using System.Collections.Generic;
using Io.AppMetrica;
using UI;
using UnityEngine;

namespace QuestsContent
{
    public class ChainTasksCounter : MonoBehaviour
    {
        [SerializeField] private List<Task> _chainTasks = new List<Task>();
        [SerializeField] private TaskUI _chainTaskUI;
        [SerializeField] private GameObject _taskLock;
        [SerializeField] private ChainTasksSaver _chainTasksSaver;

        private int _currentTaskIndex = 0;
        public Task CurrentTask { get; private set; }

        public void NextTask()
        {
            CurrentTask.ProgressSaved -= _chainTasksSaver.SaveProgress;
            Debug.Log("_currentTaskIndex " + _currentTaskIndex);
            
            AppMetrica.ReportEvent("ChainTaskCompleted", "{\"" + _currentTaskIndex + "\":null}");
            _currentTaskIndex++;
            PlayerPrefs.SetInt("CurrentChainTaskIndex", _currentTaskIndex);
            Debug.Log("NextTask " + _currentTaskIndex);

            if (CheckLockChainTasksValue())
                return;

            StartNextTask();
        }

        public void StartTask()
        {
            for (int i = 0; i < _chainTasks.Count; i++)
                _chainTasks[i].SetIndex(i);

            _currentTaskIndex = PlayerPrefs.GetInt("CurrentChainTaskIndex", _currentTaskIndex);

            if (CheckLockChainTasksValue())
                return;

            ChainTasksSaveData saveData = _chainTasksSaver.LoadProgress();

            if (saveData == null || saveData.TaskIndex != _currentTaskIndex)
            {
                Debug.Log("No progress data found. Starting from the beginning.");

                Task currentTask = _chainTasks[_currentTaskIndex];
                CurrentTask = currentTask;
                CurrentTask.ProgressSaved += _chainTasksSaver.SaveProgress;
                Debug.Log("!!!_currentTask " + CurrentTask.Index);
                currentTask.InitTaskUI(_chainTaskUI);
                currentTask.StartTask();
            }
            else
            {
                Debug.Log("!!!_currentTask LoadSaveData " + saveData.TaskIndex);

                Task currentTask = _chainTasks[_currentTaskIndex];
                CurrentTask = currentTask;
                CurrentTask.ProgressSaved += _chainTasksSaver.SaveProgress;
                CurrentTask.InitTaskUI(_chainTaskUI);
                CurrentTask.LoadProgress(saveData.CurrentValue,saveData.TargetAmount, saveData.IsCompleted, saveData.IsReceived);
            }
        }

        public void StartNextTask()
        {
            Debug.Log("No progress data found. Starting from the beginning.");
            Task currentTask = _chainTasks[_currentTaskIndex];
            CurrentTask = currentTask;
            CurrentTask.ProgressSaved += _chainTasksSaver.SaveProgress;
            Debug.Log("!!!_currentTask " + CurrentTask.Index);
            currentTask.InitTaskUI(_chainTaskUI);
            currentTask.StartTask();
        }

        private bool CheckLockChainTasksValue()
        {
            if (_currentTaskIndex >= _chainTasks.Count)
            {
                Debug.Log("All tasks completed!");
                _taskLock.SetActive(true);
                return true;
            }
            
            Debug.Log("DONT All tasks completed! " + _currentTaskIndex +" ,,, " + _chainTasks.Count);
            _taskLock.SetActive(false);
            return false;
        }
    }

    [System.Serializable]
    public class ChainTasksSaveData
    {
        public int TaskIndex;
        public string TaskID;
        public int CurrentValue;
        public int TargetAmount;
        public bool IsCompleted;
        public bool IsReceived;
    }
}