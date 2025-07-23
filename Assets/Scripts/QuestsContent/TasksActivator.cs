using System.Collections.Generic;
using UnityEngine;

namespace QuestsContent
{
    public class TasksActivator : MonoBehaviour
    {
        public static TasksActivator Instance { get; private set; }

        [SerializeField] private List<Task> _tasks = new List<Task>();

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
            StartTask();
        }

        private void StartTask()
        {
            if (_currentTaskIndex >= _tasks.Count)
            {
                Debug.Log("All tasks completed!");
                return;
            }

            Task currentTask = _tasks[_currentTaskIndex];
            _currentTask = currentTask;
            currentTask.StartTask();
        }
    }
}