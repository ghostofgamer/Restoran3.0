using System;
using LoadingSceneContent;
using UnityEngine;

namespace QuestsContent
{
    public class TasksActivator : MonoBehaviour
    {
        public static TasksActivator Instance { get; private set; }

        [SerializeField] private ChainTasksCounter _chainTasksCounter;
        [SerializeField] private DailyTasksCounter _dailyTasksCounter;

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
            _chainTasksCounter.StartTask();
            _dailyTasksCounter.StartTasks();
        }

        public void NextTask()
        {
            _chainTasksCounter.NextTask();
        }

        public void ChangeValue()
        {
            _dailyTasksCounter.ChangeValue();
        }
    }
}