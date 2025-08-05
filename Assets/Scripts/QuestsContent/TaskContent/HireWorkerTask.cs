using Enums;
using I2.Loc;
using UnityEngine;
using WorkerContent;

namespace QuestsContent.TaskContent
{
    [CreateAssetMenu(fileName = "HireWorkerTask", menuName = "QuestConfigs/HireWorkerTaskConfig", order = 1)]
    public class HireWorkerTask : Task
    {
        [SerializeField] private WorkerType _workerType;

        private Workers _workers;

        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask");
            return CurrentValue >= _targetAmount;
        }

        protected override void Initialization()
        {
            Debug.Log("--------------------InitializationTask");
            _workers = TaskInitializer.Instance.Workers;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("BUY")} {LocalizationManager.GetTermTranslation(_workerType.ToString())}";

            CurrentValue = 0;
            _languageChanger.LanguageChanged += LocalizationChanged;

            SubscribeToEvents();

            if (_isChainTask && TaskInitializer.Instance.GetWorkerHired(_workerType))
            {
                CurrentValue = _targetAmount;
                CompleteTask();
                return;
            }

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        public override void LoadProgress(int currentValue, int targetAmount, bool isCompleted, bool isReceived)
        {
            _workers = TaskInitializer.Instance.Workers;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            base.LoadProgress(currentValue, targetAmount, isCompleted, isReceived);

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("BUY")} {LocalizationManager.GetTermTranslation(_workerType.ToString())}";

            _languageChanger.LanguageChanged += LocalizationChanged;
            SubscribeToEvents();

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        protected override void SubscribeToEvents()
        {
            _workers.WorkerPurchased += ChangeValue;
            TasksUI.TaskCompleted += CloseTask;
        }

        public override void UnsubscribeFromEvents()
        {
            _workers.WorkerPurchased -= ChangeValue;
            TasksUI.TaskCompleted -= CloseTask;
        }

        public override void LocalizationChanged()
        {
            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("BUY")} {LocalizationManager.GetTermTranslation(_workerType.ToString())}";

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());
        }

        private void ChangeValue(WorkerType workerType)
        {
            Debug.Log("ServeClients " + CurrentValue);

            if (_workerType != workerType)
            {
                Debug.Log("НЕ ТОТ РАБОЧИЙ " + CurrentValue);
                return;
            }

            if (CurrentValue < _targetAmount)
                CurrentValue++;

            SaveProgress();

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount, CheckCompletion());

            if (CurrentValue >= _targetAmount)
            {
                Debug.Log("CompleteTaskServeClients " + this.name);
                CompleteTask();
            }
        }

        public override void CompleteTask()
        {
            base.CompleteTask();

            Debug.Log("!!!!!!!!!!!!!!    CompleteTaskServeClients  " + this.name + "  !  " + CurrentValue + "  !  " +
                      _targetAmount + "  !  " + CheckCompletion());

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        public override void CloseTask()
        {
            base.CloseTask();

            if (!_isChainTask)
                TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                    PrizeTask.Amount,
                    CheckCompletion());

            SaveProgress();
        }
    }
}