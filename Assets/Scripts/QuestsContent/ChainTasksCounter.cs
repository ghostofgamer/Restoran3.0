using System.Collections.Generic;
using UI;
using UnityEngine;

namespace QuestsContent
{
    public class ChainTasksCounter : MonoBehaviour
    {
        [SerializeField] private List<Task> _chainTasks = new List<Task>();
        [SerializeField] private TaskUI _chainTaskUI;
        
        private int _currentTaskIndex = 0;
        private Task _currentTask;
        
        public void NextTask()
        {
            _currentTaskIndex++;
            Debug.Log("NextTask " + _currentTaskIndex);
            StartTask();
        }

        public void StartTask()
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
            currentTask.InitTaskUI(_chainTaskUI);
            currentTask.StartTask();
        }
    }
}