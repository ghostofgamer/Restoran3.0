using UI;
using UnityEngine;

namespace QuestsContent
{
    public abstract class Task : ScriptableObject
    {
        [SerializeField] private string _taskName;
        [TextArea] [SerializeField] protected string Description;
        [SerializeField] private PrizeTask _prizeTask;

        private int _index;
        protected TaskUI ChainTasksUI;
        protected int CurrentValue;

        public bool IsCompleted { get; protected set; }
        public PrizeTask PrizeTask => _prizeTask;

        public abstract bool CheckCompletion();

        public virtual void StartTask()
        {
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
        }

        public void CloseTask()
        {
            UnsubscribeFromEvents();
            Debug.Log("ReceivePrize");
            TasksActivator.Instance.NextTask();
        }

        public void SetIndex(int index)
        {
            _index = index;
            Debug.Log("Value Index " + _index + " " + _taskName);
        }

        protected abstract void Initialization();
        protected abstract void SubscribeToEvents();
        protected abstract void UnsubscribeFromEvents();
    }
}