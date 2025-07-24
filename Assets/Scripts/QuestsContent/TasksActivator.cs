using System.Collections.Generic;
using UnityEngine;

namespace QuestsContent
{
    public class TasksActivator : MonoBehaviour
    {
        public static TasksActivator Instance { get; private set; }

        [SerializeField] private List<Task> _chainTasks = new List<Task>();

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
            currentTask.StartTask();
        }
    }
}