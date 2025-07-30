using UI;
using UnityEngine;

namespace QuestsContent
{
    public abstract class Task : ScriptableObject
    {
        [SerializeField] private string _taskId;
        [SerializeField] private string _taskName;
        [TextArea] [SerializeField] protected string Description;
        [SerializeField] private PrizeTask _prizeTask;
        [SerializeField] private bool _isChainTask;

        private DailyTasksCounter _dailyTasksCounter;
        private ChainTasksCounter _chainTasksCounter;
        private TaskPrizeRecipient _taskPrizeRecipient;
        private int _index;
        protected TaskUI TasksUI;
        public int CurrentValue;
        protected TaskData Data = null;

        public string TaskId => _taskId;
        public bool IsCompleted { get; protected set; }
        public bool IsReceived { get; protected set; } = false;
        public PrizeTask PrizeTask => _prizeTask;
        public int Index => _index;

        public abstract bool CheckCompletion();

        public virtual void InitTaskUI(TaskUI taskUI)
        {
            TasksUI = taskUI;
            _taskPrizeRecipient = TaskInitializer.Instance.TaskPrizeRecipient;

            _dailyTasksCounter = TaskInitializer.Instance.DailyTasksCounter;
            _chainTasksCounter = TaskInitializer.Instance.ChainTasksCounter;
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

        public virtual void ReceivePrize()
        {
            IsReceived = true;

            Debug.Log("ReceivePrize" + PrizeTask);
            Debug.Log("_taskPrizeRecipient" + _taskPrizeRecipient);

            _taskPrizeRecipient.ClaimPrize(PrizeTask);
            SaveProgress();
            CloseTask();
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

        public virtual void VirtualShowProgress()
        {
        }

        public virtual string SaveProgress()
        {
            TaskData data = new TaskData
            {
                taskId = _taskId,
                isCompleted = IsCompleted,
                isReceived = IsReceived,
                currentValue = CurrentValue
            };

            Debug.Log("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%Save CurrentValue " + CurrentValue + " " + _taskId);

            if (!_isChainTask && _dailyTasksCounter != null)
                _dailyTasksCounter.SaveCurrentTasks();

            if (_isChainTask && _chainTasksCounter != null)
                _chainTasksCounter.SaveCurrentTask(data);

            return JsonUtility.ToJson(data);
        }

        public virtual void LoadProgress(string json)
        {
            if (string.IsNullOrEmpty(json)) return;

            Data = JsonUtility.FromJson<TaskData>(json);
            IsCompleted = Data.isCompleted;
            IsReceived = Data.isReceived;
            CurrentValue = Data.currentValue;
            Debug.Log("Load CurrentValue " + CurrentValue + " " + _taskId);
            Debug.Log("Load IsReceived " + IsReceived );
        }

        public void ClearProgress()
        {
            IsCompleted = false;
            IsReceived = false;
            CurrentValue = 0;
            SaveProgress();
        }

        protected abstract void Initialization();

        protected abstract void SubscribeToEvents();

        protected abstract void UnsubscribeFromEvents();
    }

    [System.Serializable]
    public class TaskData
    {
        public int index;
        public string taskId;
        public bool isCompleted;
        public bool isReceived;
        public int currentValue;
    }
}