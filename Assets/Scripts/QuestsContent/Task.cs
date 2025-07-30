using System;
using NUnit.Framework.Interfaces;
using UI;
using UnityEngine;

namespace QuestsContent
{
    public abstract class Task : ScriptableObject
    {
        [SerializeField] private string  _taskID;
        [SerializeField] private string _taskName;
        [TextArea] [SerializeField] protected string Description;
        [SerializeField] private PrizeTask _prizeTask;
        [SerializeField] private bool _isChainTask;

        private int _index;
        protected TaskUI ChainTasksUI;
        protected int CurrentValue;

        public event Action ProgressSaved ;

        public bool IsCompleted { get; protected set; }
        public bool IsReceived { get; protected set; }
        
        public PrizeTask PrizeTask => _prizeTask;
        public int Index => _index;
        public string TaskID => _taskID;
        public int CurrentValueTask => CurrentValue;

        public abstract bool CheckCompletion();

        public virtual void InitTaskUI(TaskUI taskUI)
        {
            ChainTasksUI = taskUI;
        }

        public virtual void StartTask()
        {
            IsCompleted = false;
            Debug.Log("StartTask =>" + _index);
            Initialization();

            IsCompleted = CheckCompletion();

            if (IsCompleted)
                CompleteTask();
        }

        public virtual void CompleteTask()
        {
            Debug.Log("CompleteTask");
            IsCompleted = true;

            if (!_isChainTask)
                TasksActivator.Instance.ChangeValue();
        }

        public void CloseTask()
        {
            UnsubscribeFromEvents();
            Debug.Log("ReceivePrize");
            
            if (_isChainTask)
                TasksActivator.Instance.NextTask();
        }

        public void SetIndex(int index)
        {
            _index = index;
            Debug.Log("Value Index " + _index + " " + _taskName);
        }
        
        public void SaveProgress()
        {
            ProgressSaved?.Invoke();
        }

        public virtual void LoadProgress(int currentValue,bool isCompleted, bool isReceived)
        {
            CurrentValue = currentValue;
            IsCompleted = isCompleted;
            IsReceived = isReceived;
        }

        protected abstract void Initialization();
        protected abstract void SubscribeToEvents();
        protected abstract void UnsubscribeFromEvents();
    }
}