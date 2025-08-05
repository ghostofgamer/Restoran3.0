using I2.Loc;
using UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage;
using UnityEngine;

namespace QuestsContent.TaskContent
{
    [CreateAssetMenu(fileName = "BuyPlaceTask", menuName = "QuestConfigs/BuyPlaceTaskConfig", order = 1)]
    public class BuyPlaceTask : Task
    {
       [SerializeField] private int _index;

        private PlacesScrollContent _placesScrollContent;

        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask");
            return CurrentValue >= _targetAmount;
        }

        public override bool GetAdditionalConditions()
        {
            return TaskInitializer.Instance.GetBuyPlacesPossibility(_index);
        }

        protected override void Initialization()
        {
            Debug.Log("--------------------InitializationTask MAKIN MONEY");
            _placesScrollContent = TaskInitializer.Instance.PlacesScrollContent;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("BuyPlace")} #{(_index + 1)}";

            CurrentValue = 0;
            _languageChanger.LanguageChanged += LocalizationChanged;

            SubscribeToEvents();

            if (_isChainTask && TaskInitializer.Instance.GetBuyPlacesPossibility(_index))
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
            _placesScrollContent = TaskInitializer.Instance.PlacesScrollContent;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            base.LoadProgress(currentValue, targetAmount, isCompleted, isReceived);

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("BuyPlace")} #{(_index + 1)}";

            _languageChanger.LanguageChanged += LocalizationChanged;
            SubscribeToEvents();


            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        protected override void SubscribeToEvents()
        {
            _placesScrollContent.PayPlaceCompleted += ChangeValue;
            TasksUI.TaskCompleted += CloseTask;
        }

        public override void UnsubscribeFromEvents()
        {
            _placesScrollContent.PayPlaceCompleted -= ChangeValue;
            TasksUI.TaskCompleted -= CloseTask;
        }

        public override void LocalizationChanged()
        {
            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("BuyPlace")} #{(_index + 1)}";


            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount, CheckCompletion());
        }

        private void ChangeValue(int index)
        {
            Debug.Log("ChangeValue");

            if (index != _index)
            {
                Debug.Log("Неверное");
                return;
            }

            if (CurrentValue < _targetAmount)
                CurrentValue++;

            SaveProgress();

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount, CheckCompletion());

            if (CurrentValue >= _targetAmount)
            {
                Debug.Log("CompleteTaskMAkingMoney " + this.name);
                CompleteTask();
            }
        }

        public override void CompleteTask()
        {
            base.CompleteTask();

            Debug.Log("!!!!!!!!!!!!!!    CompleteTaskMAkingMoney  " + this.name + "  !  " + CurrentValue + "  !  " +
                      _targetAmount + "  !  " + CheckCompletion());

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount, CheckCompletion());

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