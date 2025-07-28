using System.Collections.Generic;
using DeliveryContent;
using EnergyContent;
using Enums;
using FortuneContent;
using MysteryGiftContent;
using PlayerContent.LevelContent;
using UI.Screens;
using UnityEngine;
using WalletContent;

namespace QuestsContent
{
    public class TaskPrizeRecipient : MonoBehaviour
    {
        [SerializeField] private Wallet _wallet;
        [SerializeField] private Energy _energy;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Fortune _fortune;
        [SerializeField] private DailyGlobalTaskPrizeScreen _dailyGlobalTaskPrizeScreen;
        [SerializeField] private Delivery _delivery;
        [SerializeField] private TaskPrizeScreen _taskPrizeScreen;

        public void ClaimPrize(PrizeTask prizeTask)
        {
            switch (prizeTask.TaskPrizeType)
            {
                case TaskPrizeType.Money:
                    _wallet.Add(new DollarValue(prizeTask.Amount, 0));
                    ShowTaskPrize(prizeTask.Icon);
                    break;

                case TaskPrizeType.XP:
                    _playerLevel.AddExp(prizeTask.Amount);
                    ShowTaskPrize(prizeTask.Icon);
                    break;

                case TaskPrizeType.Spin:
                    _fortune.AddSpins(prizeTask.Amount);
                    ShowTaskPrize(prizeTask.Icon);
                    break;

                case TaskPrizeType.Energy:
                    _energy.IncreaseEnergy(prizeTask.Amount);
                    ShowTaskPrize(prizeTask.Icon);
                    break;
            }
        }

        public void ClaimGlobalDailyPrize(List<MysteryPrize> prizes, bool isAdsShowed)
        {
            int value = 0;

            foreach (var prize in prizes)
            {
                switch (prize.MysteryPrizeType)
                {
                    case MysteryPrizeType.Money:
                        _wallet.Add(new DollarValue(prize.Value * (isAdsShowed ? 2 : 1), 00));
                        break;

                    case MysteryPrizeType.Bun:
                        _delivery.SpawnPrize(ItemType.Bun, prize.Value * (isAdsShowed ? 2 : 1));
                        break;

                    case MysteryPrizeType.Cutlet:
                        _delivery.SpawnPrize(ItemType.RawCutlet, prize.Value * (isAdsShowed ? 2 : 1));
                        break;

                    case MysteryPrizeType.PackageBurgers:
                        _delivery.SpawnPrize(ItemType.PackageBurgerPaper, prize.Value * (isAdsShowed ? 2 : 1));
                        break;

                    case MysteryPrizeType.Cheese:
                        _delivery.SpawnPrize(ItemType.Cheese, prize.Value * (isAdsShowed ? 2 : 1));
                        break;

                    case MysteryPrizeType.Tomato:
                        _delivery.SpawnPrize(ItemType.Tomato, prize.Value * (isAdsShowed ? 2 : 1));
                        break;

                    case MysteryPrizeType.Onion:
                        _delivery.SpawnPrize(ItemType.Onion, prize.Value * (isAdsShowed ? 2 : 1));
                        break;

                    case MysteryPrizeType.Salad:
                        _delivery.SpawnPrize(ItemType.Cabbage, prize.Value * (isAdsShowed ? 2 : 1));
                        break;

                    case MysteryPrizeType.Nuggets:
                        _delivery.SpawnPrize(ItemType.Nuggets, prize.Value * (isAdsShowed ? 2 : 1));
                        break;

                    case MysteryPrizeType.FrenchFries:
                        _delivery.SpawnPrize(ItemType.FrenchFries, prize.Value * (isAdsShowed ? 2 : 1));
                        break;

                    case MysteryPrizeType.PackageNuggets:
                        _delivery.SpawnPrize(ItemType.NuggetsPackage, prize.Value * (isAdsShowed ? 2 : 1));
                        break;

                    case MysteryPrizeType.PackageFries:
                        _delivery.SpawnPrize(ItemType.FrenchFriesPackage, prize.Value * (isAdsShowed ? 2 : 1));
                        break;
                }
            }
        }

        private void ShowTaskPrize(Sprite sprite)
        {
            _taskPrizeScreen.OpenScreen();
            _taskPrizeScreen.ShowReward(sprite);
        }
    }
}