using EnergyContent;
using Enums;
using NotificationContent;
using UnityEngine;
using WalletContent;

namespace TutorialContent
{
    public class TutorialNotificationActivator : MonoBehaviour
    {
        [SerializeField] private Wallet _wallet;
        [SerializeField] private Energy _energy;
        [SerializeField] private NotificationTutorialStage notificationTutorialStage;

        public void ShowNotification(TutorialPrize prize)
        {
            AcceptPrize(prize);
            notificationTutorialStage.Init(prize);
        }

        private void AcceptPrize(TutorialPrize prize)
        {
            switch (prize.PrizeType)
            {
                case TaskPrizeType.Money:
                    _wallet.Add(new DollarValue(prize.Value, 0));
                    break;
                case TaskPrizeType.Energy:
                    _energy.IncreaseEnergy(prize.Value);
                    break;
            }
        }
    }
}