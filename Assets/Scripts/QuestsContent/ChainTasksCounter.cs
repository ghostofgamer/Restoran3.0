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
            
            if (PlayerPrefs.HasKey("TaskChainProgress" + _currentTask.Index))
            {
                string jsonData = PlayerPrefs.GetString("TaskChainProgress" + _currentTask.Index);
                currentTask.LoadProgress(jsonData);
            }
            else
            {
                currentTask.ClearProgress();
                currentTask.StartTask();
            }
        }

        public void SaveCurrentTask(TaskData data)
        {
            string jsonData = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("TaskChainProgress" + _currentTask.Index, jsonData);
            PlayerPrefs.Save();
            Debug.Log("Saved Task: " + jsonData);
        }
    }
}