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
            Debug.Log(" CurrentValue = Data.currentValue " + Data.currentValue);
            CurrentValue = 0;

            Init();
        }

        public void Init()
        {
            _assemblyBurger = TaskInitializer.Instance.AssemblyBurger;
            _assemblyFromDeepFry = TaskInitializer.Instance.AssemblyFromDeepFry;
            SubscribeToEvents();
            TasksUI.ChangeValue(this, Description, CurrentValue, _targetAmount, PrizeTask.Icon, PrizeTask.Amount,
                CheckCompletion(), false);

            Debug.Log("ТУУУУУУУт " + TaskId + "  ???  " + CheckCompletion());
        }

        protected override void SubscribeToEvents()
        {
            _assemblyBurger.BurgerCreated += ChangeValue;
            _assemblyFromDeepFry.ItemCreated += ChangeValue;
        }

        protected override void UnsubscribeFromEvents()
        {
            _assemblyBurger.BurgerCreated -= ChangeValue;
            _assemblyFromDeepFry.ItemCreated -= ChangeValue;
        }

        private void ChangeValue(Item item)
        {
            if (item.ItemType == _itemType)
            {
                if (CurrentValue >= _targetAmount)
                    return;

                CurrentValue++;

                SaveProgress();

                TasksUI.ChangeValue(this, Description, CurrentValue, _targetAmount, PrizeTask.Icon,
                    PrizeTask.Amount, CheckCompletion(), false);

                if (CurrentValue >= _targetAmount)
                    CompleteTask();
            }
            else
            {
                Debug.Log("Не верный тип Item ");
            }
        }

        public override void LoadProgress(string json)
        {
            base.LoadProgress(json);
            Init();
        }

        public override void VirtualShowProgress()
        {
            base.VirtualShowProgress();

            Debug.Log("метод VirtualShowProgress" + CurrentValue);

            TasksUI.ChangeValue(this, Description, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount, CheckCompletion(), IsReceived);
        }

        public override void CompleteTask()
        {
            TasksUI.ChangeValue(this, Description, CurrentValue, _targetAmount, PrizeTask.Icon, PrizeTask.Amount,
                CheckCompletion(), false);
            base.CompleteTask();
            SaveProgress();
        }
    }
}