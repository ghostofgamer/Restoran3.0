using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyBurgerContent;
using Enums;
using I2.Loc;
using ItemContent;
using KitchenEquipmentContent.FryerContent;
using PlayerContent.LevelContent;
using SoContent;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QuestsContent
{
    [CreateAssetMenu(fileName = "MakeItems", menuName = "QuestConfigs/MakeItemsConfig", order = 1)]
    public class MakeItemsTask : Task
    {
        [SerializeField] private int _minTargetValue;
        [SerializeField] private int _maxTargetValue;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private ItemMakeTask[] _itemMakeTaskInfos;
        [SerializeField] private ItemsConfig _itemsConfig;

        private PlayerLevel _playerLevel;
        private AssemblyBurger _assemblyBurger;
        private AssemblyFromDeepFry _assemblyFromDeepFry;
        private Ingredient _ingredient;
        
        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask " + (CurrentValue >= _targetAmount));
            return CurrentValue >= _targetAmount;
        }

        protected override void Initialization()
        {
            _playerLevel = TaskInitializer.Instance.PlayerLevel;
            _languageChanger = TaskInitializer.Instance.LanguageChanger;
            _assemblyBurger = TaskInitializer.Instance.AssemblyBurger;
            _assemblyFromDeepFry = TaskInitializer.Instance.AssemblyFromDeepFry;

            if (!_isChainTask)
            {
                _targetAmount = Random.Range(_minTargetValue, _maxTargetValue);
                _itemType = GetItemType();

                PlayerPrefs.SetInt("MakeItemsTaskSaveItem",(int)_itemType);
            }

            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("Cook")} {LocalizationManager.GetTermTranslation(_itemsConfig.GetItemConfig(_itemType).Term)} ({_targetAmount})";

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
                int savedItemType = PlayerPrefs.GetInt("MakeItemsTaskSaveItem", (int)ItemType.FinishSmallBurger);
                _itemType = (ItemType)savedItemType;
            }
            
            _localizationDescription =
                $"{LocalizationManager.GetTermTranslation("Cook")} {LocalizationManager.GetTermTranslation(_itemsConfig.GetItemConfig(_itemType).Term)} ({_targetAmount})";

            _languageChanger = TaskInitializer.Instance.LanguageChanger;
            _languageChanger.LanguageChanged += LocalizationChanged;
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
            {
                throw new InvalidOperationException("Нет подходящих типов предметов для текущего уровня игрока.");
            }
            
            Debug.Log("GetItemType " + suitableItemTypes.Count);

            foreach (var typed in suitableItemTypes)
            {
                Debug.Log("typed " + typed);
            }

            int randomIndex = Random.Range(0, suitableItemTypes.Count);
            return suitableItemTypes[randomIndex];
        }
    }

    [Serializable]
    public class ItemMakeTask
    {
        public int Level;
        public ItemType ItemType;
    }
}