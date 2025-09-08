using I2.Loc;
using OrdersContent;
using UnityEngine;

namespace QuestsContent.TaskContent
{
    [CreateAssetMenu(fileName = "ServeClientsTask", menuName = "QuestConfigs/ServeClientsTaskConfig", order = 1)]
    public class ServeClientsTask : Task
    {
        [SerializeField] private int _minTargerValue;
        [SerializeField] private int _maxTargerValue;
        
        private OrdersCounter _ordersCounter;
        
        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask");
            return CurrentValue >= _targetAmount;
        }

        protected override void Initialization()
        {
            Debug.Log("--------------------InitializationTask MAKIN MONEY");
            _ordersCounter = TaskInitializer.Instance.OrdersCounter;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;
            
            if (!_isChainTask)
                _targetAmount = Random.Range(_minTargerValue, _maxTargerValue);

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("ServeClients")} ({_targetAmount})";


            CurrentValue = 0;
            _languageChanger.LanguageChanged += LocalizationChanged;

            SubscribeToEvents();

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }
        
        public override void LoadProgress(int currentValue, int targetAmount, bool isCompleted, bool isReceived)
        {
            _ordersCounter = TaskInitializer.Instance.OrdersCounter;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            base.LoadProgress(currentValue, targetAmount, isCompleted, isReceived);

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("ServeClients")} ({_targetAmount})";

            _languageChanger.LanguageChanged += LocalizationChanged;
            SubscribeToEvents();


            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        protected override void SubscribeToEvents()
        {
            // _ordersCounter.OrderCompleted += ChangeValue;
            _ordersCounter.OrderFinished += ChangeValue;
            TasksUI.TaskCompleted += CloseTask;
        }

        public override void UnsubscribeFromEvents()
        {
            _ordersCounter.OrderFinished -= ChangeValue;
            // _ordersCounter.OrderCompleted -= ChangeValue;
            TasksUI.TaskCompleted -= CloseTask;
        }

        public override void LocalizationChanged()
        {
            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("ServeClients")} ({_targetAmount})";
            

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());
        }
        
        private void ChangeValue()
        {
            Debug.Log("ServeClients "+CurrentValue);

            if (CurrentValue < _targetAmount)
                CurrentValue ++;

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

        public override void tESTcOMPLETED()
        {
            base.CompleteTask();
            TasksUI.ChangeValue(this, _localizationDescription, _targetAmount, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount, true);
            SaveProgress();
        }

        public override void CloseTask()
        {
            base.CloseTask();
            
            if (!_isChainTask)
                TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                    PrizeTask.Amount, CheckCompletion());

            SaveProgress();
        }
    }
}