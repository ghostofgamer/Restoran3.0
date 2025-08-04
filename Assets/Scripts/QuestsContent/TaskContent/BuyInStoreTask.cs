using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using I2.Loc;
using PlayerContent.LevelContent;
using SoContent;
using UI.Screens.ShopContent;
using UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QuestsContent.TaskContent
{
    [CreateAssetMenu(fileName = "BuyInStoreTask", menuName = "QuestConfigs/BuyInStoreTaskConfig", order = 1)]
    public class BuyInStoreTask : Task
    {
        [SerializeField] private int _minTargetValue;
        [SerializeField] private int _maxTargetValue;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private ItemMakeTask[] _itemMakeTaskInfos;
        [SerializeField] private ItemsConfig _itemsConfig;

        private PlayerLevel _playerLevel;
        private ItemCartScroll _itemCartScroll;

        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask");
            return CurrentValue >= _targetAmount;
        }

        protected override void Initialization()
        {
            _playerLevel = TaskInitializer.Instance.PlayerLevel;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;
            _itemCartScroll = TaskInitializer.Instance.ItemCartScroll;

            if (!_isChainTask)
            {
                _targetAmount = Random.Range(_minTargetValue, _maxTargetValue);
                _itemType = GetItemType();

                PlayerPrefs.SetInt("BuyItemsTaskSaveItem", (int) _itemType);
            }

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("BuyInStore")} {LocalizationManager.GetTermTranslation(_itemsConfig.GetItemConfig(_itemType).Term)} ({_targetAmount})";

            _languageChanger.LanguageChanged += LocalizationChanged;
            CurrentValue = 0;

            SubscribeToEvents();
            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        public override void LoadProgress(int currentValue, int targetAmount, bool isCompleted, bool isReceived)
        {
            base.LoadProgress(currentValue, targetAmount, isCompleted, isReceived);

            if (!_isChainTask)
            {
                int savedItemType = PlayerPrefs.GetInt("BuyItemsTaskSaveItem", (int) ItemType.Bun);
                _itemType = (ItemType) savedItemType;
            }

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("BuyInStore")} {LocalizationManager.GetTermTranslation(_itemsConfig.GetItemConfig(_itemType).Term)} ({_targetAmount})";

            _languageChanger = TaskInitializer.Instance.LanguageChanger;
            _itemCartScroll = TaskInitializer.Instance.ItemCartScroll;

            _languageChanger.LanguageChanged += LocalizationChanged;
            SubscribeToEvents();
            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());

            SaveProgress();
        }

        protected override void SubscribeToEvents()
        {
            TasksUI.TaskCompleted += CloseTask;
            _itemCartScroll.ItemsPurchased += ChangeValue;
        }

        public override void UnsubscribeFromEvents()
        {
            TasksUI.TaskCompleted -= CloseTask;
            _itemCartScroll.ItemsPurchased -= ChangeValue;
        }

        private void ChangeValue(List<ItemCart> itemCarts)
        {
            foreach (var itemCart in itemCarts)
            {
                if (itemCart.ItemType == _itemType)
                {
                    if (CurrentValue >= _targetAmount)
                        return;

                    CurrentValue += itemCart.CurrentAmount;
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
        }

        public override void CompleteTask()
        {
            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());
            base.CompleteTask();
            SaveProgress();
        }

        public override void LocalizationChanged()
        {
            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("Cook")} {LocalizationManager.GetTermTranslation(_itemsConfig.GetItemConfig(_itemType).Term)} ({_targetAmount})";

            Debug.Log("_localizationDescription  LocalizationChanged " + _localizationDescription);

            TasksUI.ChangeValue(this, _localizationDescription, CurrentValue, _targetAmount, PrizeTask.Icon,
                PrizeTask.Amount,
                CheckCompletion());
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