using System;
using NUnit.Framework.Interfaces;
using UI;
using UnityEngine;

namespace QuestsContent
{
    public abstract class Task : ScriptableObject
    {
        [SerializeField] private string _taskID;
        [SerializeField] private string _taskName;
        [TextArea] [SerializeField] protected string Description;
        [SerializeField] private PrizeTask _prizeTask;
        [SerializeField] private bool _isChainTask;
        [SerializeField] private int _targetAmount;

        private int _index;
        protected string _localizationDescription;

        protected TaskUI TasksUI;
        protected int CurrentValue;

        public event Action ProgressSaved;

        public int TargetAmount => _targetAmount;
        public string LocalizationDescription => _localizationDescription;
        public bool IsCompleted { get; protected set; }
        public bool IsReceived { get; protected set; } = false;

        public PrizeTask PrizeTask => _prizeTask;
        public int Index => _index;
        public string TaskID => _taskID;
        public int CurrentValueTask => CurrentValue;

        public abstract bool CheckCompletion();

        public virtual void InitTaskUI(TaskUI taskUI)
        {
            TasksUI = taskUI;
        }

        public virtual void StartTask()
        {
            ResetTaskState();
            /*IsReceived = false;
            IsCompleted = false;*/
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
            IsReceived = true;
            SaveProgress();

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

        public virtual void LoadProgress(int currentValue,int targetAmount, bool isCompleted, bool isReceived)
        {
            ResetTaskState();
            CurrentValue = currentValue;
            _targetAmount = targetAmount;
            IsCompleted = isCompleted;
            IsReceived = isReceived;
        }

        [ContextMenu("Reset Task State")]
        public void ResetTaskState()
        {
            IsCompleted = false;
            IsReceived = false;
            CurrentValue = 0;
            Debug.Log(
                $"Task {_taskID} state reset: IsReceived={IsReceived}, IsCompleted={IsCompleted}, CurrentValue={CurrentValue}");
        }

        protected abstract void Initialization();
        protected abstract void SubscribeToEvents();
        public abstract void UnsubscribeFromEvents();
    }
}