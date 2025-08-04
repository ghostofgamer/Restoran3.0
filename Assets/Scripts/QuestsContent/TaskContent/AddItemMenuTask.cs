using Enums;
using I2.Loc;
using RestaurantContent.MenuContent;
using SoContent;
using UnityEngine;

namespace QuestsContent.TaskContent
{
    [CreateAssetMenu(fileName = "AddItemMenuTask", menuName = "QuestConfigs/AddItemMenuTaskConfig", order = 1)]
    public class AddItemMenuTask : Task
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private ItemsConfig _itemsConfig;

        private MenuCounter _menuCounter;

        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask");
            return CurrentValue >= _targetAmount;
        }

        protected override void Initialization()
        {
            Debug.Log("--------------------InitializationTask AddItemToMenu");
            _menuCounter = TaskInitializer.Instance.MenuCounter;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("AddItemMenu")} {LocalizationManager.GetTermTranslation(_itemsConfig.GetItemConfig(_itemType).Term)}";

            CurrentValue = 0;
            _languageChanger.LanguageChanged += LocalizationChanged;

            SubscribeToEvents();

            if (TaskInitializer.Instance.GetItemToMenuUsing(_itemType))
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
            _menuCounter = TaskInitializer.Instance.MenuCounter;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            base.LoadProgress(currentValue, targetAmount, isCompleted, isReceived);

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("AddItemMenu")} {LocalizationManager.GetTermTranslation(_itemsConfig.GetItemConfig(_itemType).Term)}";

            _languageChanger.LanguageChanged += LocalizationChanged;
            SubscribeToEvents();

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        protected override void SubscribeToEvents()
        {
            _menuCounter.ItemAdded += ChangeValue;
            TasksUI.TaskCompleted += CloseTask;
        }

        public override void UnsubscribeFromEvents()
        {
            _menuCounter.ItemAdded -= ChangeValue;
            TasksUI.TaskCompleted -= CloseTask;
        }

        public override void LocalizationChanged()
        {
            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("AddItemMenu")} {LocalizationManager.GetTermTranslation(_itemsConfig.GetItemConfig(_itemType).Term)}";

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());
        }

        private void ChangeValue(ItemType itemType)
        {
            Debug.Log("ChangeValue Energy ");

            if (itemType != _itemType)
                return;

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

            Debug.Log("!!!!!!!!!!!!!!    CompleteTaskMAkingEnergy  " + this.name + "  !  " + CurrentValue + "  !  " +
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