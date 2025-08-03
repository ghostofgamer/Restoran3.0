using FortuneContent;
using I2.Loc;
using UnityEngine;

namespace QuestsContent.TaskContent
{
    [CreateAssetMenu(fileName = "SpinWheelFortuneTask", menuName = "QuestConfigs/SpinWheelFortuneTaskConfig", order = 1)]
    public class SpinWheelFortuneTask : Task
    {
        [SerializeField] private int _minTargetValue;
        [SerializeField] private int _maxTargetValue;
        
        private Fortune _fortune;
        
        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask");
            return CurrentValue >= _targetAmount;
        }

        protected override void Initialization()
        {
            Debug.Log("--------------------InitializationTask SpinWheel");
            _fortune = TaskInitializer.Instance.Fortune;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;
            
            if (!_isChainTask)
                _targetAmount = Random.Range(_minTargetValue, _maxTargetValue);

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("SpinWheelFortune")} ({_targetAmount})";


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
            _fortune = TaskInitializer.Instance.Fortune;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            base.LoadProgress(currentValue, targetAmount, isCompleted, isReceived);

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("SpinWheelFortune")} {_targetAmount}";

            _languageChanger.LanguageChanged += LocalizationChanged;
            SubscribeToEvents();


            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        protected override void SubscribeToEvents()
        {
            _fortune.SpinUsed += ChangeValue;
            TasksUI.TaskCompleted += CloseTask;
        }

        public override void UnsubscribeFromEvents()
        {
            _fortune.SpinUsed -= ChangeValue;
            TasksUI.TaskCompleted -= CloseTask;
        }

        public override void LocalizationChanged()
        {
            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("SpinWheelFortune")} {_targetAmount}";
            
            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());
        }
        
        private void ChangeValue()
        {
            Debug.Log("ChangeValue Spin fortunew wheel ");
            
            if (CurrentValue < _targetAmount)
                CurrentValue++;

            SaveProgress();

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount, CheckCompletion());

            if (CurrentValue >= _targetAmount)
            {
                Debug.Log("CompleteTaskSpinjFortune " + this.name);
                CompleteTask();
            }
        }

        public override void CompleteTask()
        {
            base.CompleteTask();

            Debug.Log("!!!!!!!!!!!!!!    CompleteTaskSpinjFortune " + this.name + "  !  " + CurrentValue + "  !  " +
                      _targetAmount + "  !  " + CheckCompletion());

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        public override void CloseTask()
        {
            base.CloseTask();
            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }
    }
}