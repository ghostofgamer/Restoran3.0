using System.Collections.Generic;
using UI;
using UnityEngine;

namespace QuestsContent
{
    public class ChainTasksCounter : MonoBehaviour
    {
        private const string TASK_PROGRESS_KEY_PREFIX = "TaskProgress";
        
        [SerializeField] private List<Task> _chainTasks = new List<Task>();
        [SerializeField] private TaskUI _chainTaskUI;
        [SerializeField] private GameObject _chainTaskCompleteLock;   

        private int _currentTaskIndex = 0;
        private Task _currentTask;

        public void NextTask()
        {
            _currentTaskIndex++;

            PlayerPrefs.SetInt("CurrentChainTaskIndex", _currentTaskIndex);
            Debug.Log("NextTask " + _currentTaskIndex);
            StartTask();
        }

        public void StartTask()
        {
            for (int i = 0; i < _chainTasks.Count; i++)
                _chainTasks[i].SetIndex(i);

            _currentTaskIndex = PlayerPrefs.GetInt("CurrentChainTaskIndex", 0);

            if (_currentTaskIndex >= _chainTasks.Count)
            {
                _chainTaskCompleteLock.SetActive(true);
                Debug.Log("All tasks completed!");
                return;
            }
            
            _chainTaskCompleteLock.SetActive(false);

            Task currentTask = _chainTasks[_currentTaskIndex];
            _currentTask = currentTask;
            currentTask.InitTaskUI(_chainTaskUI);
            currentTask.StartTask();
            // LoadCurrentChainTask();
        }
        
        public void SaveCurrentTask()
        {
            /*if (_currentTask == null || _currentTaskIndex >= _chainTasks.Count)
            {
                Debug.LogWarning("No task to save or all tasks completed.");
                return;
            }
            
            string taskProgressJson = _currentTask.SaveProgress();
            PlayerPrefs.SetString(TASK_PROGRESS_KEY_PREFIX + _currentTask.TaskId, taskProgressJson);
            PlayerPrefs.Save();
            Debug.Log($"Saved task progress for task: {_currentTask.TaskId}, JSON: {taskProgressJson}");*/
        }

        private void LoadCurrentChainTask()
        {
            /*string taskProgressJson = PlayerPrefs.GetString(TASK_PROGRESS_KEY_PREFIX + _currentTask.TaskId, "");
            
            if (!string.IsNullOrEmpty(taskProgressJson))
            {
                _currentTask.LoadProgress(taskProgressJson);
                Debug.Log($"Loaded task progress for task: {_currentTask.TaskId}, JSON: {taskProgressJson}");
            }
            else
            {
                Debug.Log($"No saved progress found for task: {_currentTask.TaskId}. Starting fresh.");
                _currentTask.ClearProgress();
            }*/
        }
    }
}