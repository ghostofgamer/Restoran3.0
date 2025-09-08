using Enums;
using I2.Loc;
using UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage;
using UnityEngine;

namespace QuestsContent.TaskContent
{
    [CreateAssetMenu(fileName = "BuyEquipmentTask", menuName = "QuestConfigs/BuyEquipmentTaskConfig", order = 1)]
    public class BuyEquipmentTask : Task
    {
        [SerializeField] private EquipmentType _equipmentType;

        private EquipmentScrollContent _equipmentScrollContent;

        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask");
            return CurrentValue >= _targetAmount;
        }

        protected override void Initialization()
        {
            Debug.Log("--------------------InitializationTask");
            _equipmentScrollContent = TaskInitializer.Instance.EquipmentScrollContent;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("BUY")} {LocalizationManager.GetTermTranslation(_equipmentType.ToString())} ({_targetAmount})";

            CurrentValue = 0;
            _languageChanger.LanguageChanged += LocalizationChanged;

            SubscribeToEvents();

            if (_isChainTask && TaskInitializer.Instance.GetValueNotPurchasedEquipment(_equipmentType))
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
            _equipmentScrollContent = TaskInitializer.Instance.EquipmentScrollContent;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            base.LoadProgress(currentValue, targetAmount, isCompleted, isReceived);

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("BUY")} {LocalizationManager.GetTermTranslation(_equipmentType.ToString())} ({_targetAmount})";

            _languageChanger.LanguageChanged += LocalizationChanged;
            SubscribeToEvents();

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        protected override void SubscribeToEvents()
        {
            _equipmentScrollContent.BuyEquipmentCompleted += ChangeValue;
            TasksUI.TaskCompleted += CloseTask;
        }

        public override void UnsubscribeFromEvents()
        {
            _equipmentScrollContent.BuyEquipmentCompleted -= ChangeValue;
            TasksUI.TaskCompleted -= CloseTask;
        }

        public override void LocalizationChanged()
        {
            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("BUY")} {LocalizationManager.GetTermTranslation(_equipmentType.ToString())} ({_targetAmount})";

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());
        }

        private void ChangeValue(EquipmentType equipmentType)
        {
            Debug.Log("ServeClients " + CurrentValue);

            if (equipmentType != _equipmentType)
            {
                Debug.Log("НЕ ТОТ ЕКВИПМЕНТ " + CurrentValue);
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
                    PrizeTask.Amount,
                    CheckCompletion());

            SaveProgress();
        }
    }
}