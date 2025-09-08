using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using I2.Loc;
using OrdersContent;
using PlayerContent.LevelContent;
using SoContent;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QuestsContent.TaskContent
{
    [CreateAssetMenu(fileName = "SellItemTask", menuName = "QuestConfigs/SellItemTaskConfig", order = 1)]
    public class SellItemTask : Task
    {
        [SerializeField] private int _minTargerValue;
        [SerializeField] private int _maxTargerValue;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private ItemMakeTask[] _itemMakeTaskInfos;
        [SerializeField] private ItemsConfig _itemsConfig;

        private OrdersCounter _ordersCounter;
        private PlayerLevel _playerLevel;

        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask");
            return CurrentValue >= _targetAmount;
        }

        protected override void Initialization()
        {
            Debug.Log("--------------------InitializationTask MAKIN MONEY");
            _playerLevel = TaskInitializer.Instance.PlayerLevel;
            _ordersCounter = TaskInitializer.Instance.OrdersCounter;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;

            if (!_isChainTask)
            {
                _targetAmount = Random.Range(_minTargerValue, _maxTargerValue);
                _itemType = GetItemType();
                PlayerPrefs.SetInt("SellItemTaskSaveItem", (int) _itemType);
            }

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("Sell")} {LocalizationManager.GetTermTranslation(_itemsConfig.GetItemConfig(_itemType).Term)} ({_targetAmount})";

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
            _playerLevel = TaskInitializer.Instance.PlayerLevel;

            base.LoadProgress(currentValue, targetAmount, isCompleted, isReceived);

            if (!_isChainTask)
            {
                int savedItemType = PlayerPrefs.GetInt("SellItemTaskSaveItem", (int) ItemType.Coffee);
                _itemType = (ItemType) savedItemType;
            }

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("Sell")} {LocalizationManager.GetTermTranslation(_itemsConfig.GetItemConfig(_itemType).Term)} ({_targetAmount})";

            _languageChanger.LanguageChanged += LocalizationChanged;
            SubscribeToEvents();

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        protected override void SubscribeToEvents()
        {
            _ordersCounter.CurrentOrderFinished += ChangeValue;
            TasksUI.TaskCompleted += CloseTask;
        }

        public override void UnsubscribeFromEvents()
        {
            _ordersCounter.CurrentOrderFinished -= ChangeValue;
            TasksUI.TaskCompleted -= CloseTask;
        }

        public override void LocalizationChanged()
        {
            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("Sell")} {LocalizationManager.GetTermTranslation(_itemsConfig.GetItemConfig(_itemType).Term)} ({_targetAmount})";


            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());
        }

        private void ChangeValue(Order order)
        {
            Debug.Log("ServeClients " + CurrentValue);

            int value = 0;

            if (order.DrinkItemOrder != null && order.DrinkItemOrder == _itemType)
            {
                Debug.Log("Напиток ВЕРНЫЙ " + _itemType);
                value++;
            }

            if (order.ExtraItemOrder != null && order.ExtraItemOrder == _itemType)
            {
                Debug.Log("ЕКСТРА ВЕРНЫЙ " + _itemType);
                value++;
            }

            if (order.BurgerItemOrder != null && order.BurgerItemOrder == _itemType)
            {
                Debug.Log("Бургер ВЕРНЫЙ " + _itemType);
                value++;
            }

            if (value <= 0)
            {
                Debug.Log("НЕВЕРНЫЙ ЗАКАЗ  ТУТ");
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
                    PrizeTask.Amount, CheckCompletion());

            SaveProgress();
        }

        private ItemType GetItemType()
        {
            List<ItemType> suitableItemTypes = _itemMakeTaskInfos
                .Where(task => task.Level <= _playerLevel.CurrentLevel)
                .Select(task => task.ItemType)
                .ToList();

            if (suitableItemTypes.Count == 0)
                throw new InvalidOperationException("Нет подходящих типов предметов для текущего уровня игрока.");

            Debug.Log("GetItemType " + suitableItemTypes.Count);

            foreach (var typed in suitableItemTypes)
                Debug.Log("typed " + typed);

            int randomIndex = Random.Range(0, suitableItemTypes.Count);
            return suitableItemTypes[randomIndex];
        }
    }
}