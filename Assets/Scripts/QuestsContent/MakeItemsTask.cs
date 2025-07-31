using AssemblyBurgerContent;
using Enums;
using I2.Loc;
using KitchenEquipmentContent.FryerContent;
using UI;
using UnityEngine;

namespace QuestsContent
{
    [CreateAssetMenu(fileName = "MakeItems", menuName = "QuestConfigs/MakeItemsConfig", order = 1)]
    public class MakeItemsTask : Task
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private int _targetAmount;

        private AssemblyBurger _assemblyBurger;
        private AssemblyFromDeepFry _assemblyFromDeepFry;

        public override void InitTaskUI(TaskUI taskUI)
        {
            base.InitTaskUI(taskUI);
        }

        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask " + (CurrentValue >= _targetAmount));
            return CurrentValue >= _targetAmount;
        }

        protected override void Initialization()
        {
            _targetAmount = Random.Range(10, 25);
            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation(_itemType.ToString())} {LocalizationManager.GetTermTranslation(_itemType.ToString())} {_targetAmount}";
            
            
            CurrentValue = 0;
            _assemblyBurger = TaskInitializer.Instance.AssemblyBurger;
            _assemblyFromDeepFry = TaskInitializer.Instance.AssemblyFromDeepFry;
            // ChainTasksUI = TaskInitializer.Instance.ChainTaskUI;
            SubscribeToEvents();
            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        public override void LoadProgress(int currentValue, int targetAmount, bool isCompleted, bool isReceived)
        {
            base.LoadProgress(currentValue, targetAmount, isCompleted, isReceived);
            
            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("Cook")} {LocalizationManager.GetTermTranslation(_itemType.ToString())} {_targetAmount}";
            
            _assemblyBurger = TaskInitializer.Instance.AssemblyBurger;
            _assemblyFromDeepFry = TaskInitializer.Instance.AssemblyFromDeepFry;
            SubscribeToEvents();
            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        protected override void SubscribeToEvents()
        {
            _assemblyBurger.BurgerCreated += ChangeValue;
            _assemblyFromDeepFry.ItemCreated += ChangeValue;
            TasksUI.TaskCompleted += CloseTask;
        }

        public override void UnsubscribeFromEvents()
        {
            _assemblyBurger.BurgerCreated -= ChangeValue;
            _assemblyFromDeepFry.ItemCreated -= ChangeValue;
            TasksUI.TaskCompleted -= CloseTask;
        }

        private void ChangeValue(Item item)
        {
            if (item.ItemType == _itemType)
            {
                if (CurrentValue >= _targetAmount)
                    return;

                CurrentValue++;
                SaveProgress();

                TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                    PrizeTask.Amount, CheckCompletion());

                if (CurrentValue >= _targetAmount)
                    CompleteTask();
            }
            else
            {
                Debug.Log("Не верный тип Item ");
            }
        }

        public override void CompleteTask()
        {
            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());
            base.CompleteTask();
            SaveProgress();
        }
    }
}