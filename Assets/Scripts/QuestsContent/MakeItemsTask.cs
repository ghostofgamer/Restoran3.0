using AssemblyBurgerContent;
using Enums;
using KitchenEquipmentContent.FryerContent;
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

        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask " + (CurrentValue >= _targetAmount));
            return CurrentValue >= _targetAmount;
        }

        protected override void Initialization()
        {
            CurrentValue = 0;
            _assemblyBurger = TaskInitializer.Instance.AssemblyBurger;
            _assemblyFromDeepFry = TaskInitializer.Instance.AssemblyFromDeepFry;
            // ChainTasksUI = TaskInitializer.Instance.ChainTaskUI;
            SubscribeToEvents();
            ChainTasksUI.ChangeValue(this, Description, CurrentValue, _targetAmount, PrizeTask.Icon, PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        public override void LoadProgress(int currentValue, bool isCompleted, bool isReceived)
        {
            base.LoadProgress(currentValue, isCompleted, isReceived);
            _assemblyBurger = TaskInitializer.Instance.AssemblyBurger;
            _assemblyFromDeepFry = TaskInitializer.Instance.AssemblyFromDeepFry;
            SubscribeToEvents();
            ChainTasksUI.ChangeValue(this, Description, CurrentValue, _targetAmount, PrizeTask.Icon, PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        protected override void SubscribeToEvents()
        {
            _assemblyBurger.BurgerCreated += ChangeValue;
            _assemblyFromDeepFry.ItemCreated += ChangeValue;
            ChainTasksUI.TaskCompleted += CloseTask;
        }

        protected override void UnsubscribeFromEvents()
        {
            _assemblyBurger.BurgerCreated -= ChangeValue;
            _assemblyFromDeepFry.ItemCreated -= ChangeValue;
            ChainTasksUI.TaskCompleted -= CloseTask;
        }

        private void ChangeValue(Item item)
        {
            if (item.ItemType == _itemType)
            {
                if (CurrentValue >= _targetAmount)
                    return;

                CurrentValue++;
                SaveProgress();

                ChainTasksUI.ChangeValue(this, Description, CurrentValue, _targetAmount, PrizeTask.Icon,
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
            ChainTasksUI.ChangeValue(this, Description, CurrentValue, _targetAmount, PrizeTask.Icon, PrizeTask.Amount,
                CheckCompletion());
            base.CompleteTask();
            SaveProgress();
        }
    }
}