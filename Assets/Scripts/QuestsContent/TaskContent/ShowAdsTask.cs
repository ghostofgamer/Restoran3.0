using ADSContent;
using I2.Loc;
using UnityEngine;

namespace QuestsContent.TaskContent
{
    [CreateAssetMenu(fileName = "ShowAdsTask", menuName = "QuestConfigs/ShowAdsConfig", order = 1)]
    public class ShowAdsTask : Task
    {
        [SerializeField] private int _minTarget;
        [SerializeField] private int _maxTarget;
        
        private ADS _ads;
        
        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask");
            return CurrentValue >= _targetAmount;
        }

        protected override void Initialization()
        {
            Debug.Log("--------------------InitializationTask ShowedADS");
            _ads = TaskInitializer.Instance.ADS;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;
            
            if (!_isChainTask)
                _targetAmount = Random.Range(_minTarget, _maxTarget);

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("ShowedADS")} ({_targetAmount})";
            
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
            _ads = TaskInitializer.Instance.ADS;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            base.LoadProgress(currentValue, targetAmount, isCompleted, isReceived);

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("ShowedADS")} ({_targetAmount})";

            _languageChanger.LanguageChanged += LocalizationChanged;
            SubscribeToEvents();


            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        protected override void SubscribeToEvents()
        {
            _ads.AdsShowed += ChangeValue;
            TasksUI.TaskCompleted += CloseTask;
        }

        public override void UnsubscribeFromEvents()
        {
            _ads.AdsShowed -= ChangeValue;
            TasksUI.TaskCompleted -= CloseTask;
        }

        public override void LocalizationChanged()
        {
            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("ShowedADS")} ({_targetAmount})";
            

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());
        }
        
        private void ChangeValue()
        {
            Debug.Log("ChangeValue ShowedADS ");

            if (CurrentValue < _targetAmount)
                CurrentValue ++;

            SaveProgress();

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount, CheckCompletion());

            if (CurrentValue >= _targetAmount)
            {
                Debug.Log("CompleteTaskShowedADS " + this.name);
                CompleteTask();
            }
        }
        
        public override void CompleteTask()
        {
            base.CompleteTask();

            Debug.Log("!!!!!!!!!!!!!!    CompleteTaskMAkingMoney  " + this.name + "  !  " + CurrentValue + "  !  " +
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