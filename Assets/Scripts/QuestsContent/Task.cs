using System;
using SettingsContent;
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
        [SerializeField] protected bool _isChainTask;
        [SerializeField] protected int _targetAmount;


        protected string _localizationDescription;
        protected LanguageChanger _languageChanger;
        protected TaskUI TasksUI;
        protected int CurrentValue;

        private int _index;
        
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

            Debug.Log("StartTask =>" + _index);
            Initialization();

            IsCompleted = CheckCompletion();

            if (IsCompleted)
                CompleteTask();
        }

        public virtual void CompleteTask()
        {
            Debug.Log("CompleteTask" + this._taskID);
            IsCompleted = true;

            if (!_isChainTask)
                TasksActivator.Instance.ChangeValue();
        }

        public virtual void CloseTask()
        {
            UnsubscribeFromEvents();
            IsReceived = true;
            Debug.Log("ReceivePrize " + _taskID);
            
            SaveProgress();
            _languageChanger.LanguageChanged -= LocalizationChanged;

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

        public virtual void LoadProgress(int currentValue, int targetAmount, bool isCompleted, bool isReceived)
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

            if (_languageChanger != null)
                _languageChanger.LanguageChanged -= LocalizationChanged;

            Debug.Log(
                $"Task {_taskID} state reset: IsReceived={IsReceived}, IsCompleted={IsCompleted}, CurrentValue={CurrentValue}");
        }

        protected abstract void Initialization();
        protected abstract void SubscribeToEvents();
        public abstract void UnsubscribeFromEvents();
        public abstract void LocalizationChanged();
    }
}