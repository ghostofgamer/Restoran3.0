using EnergyContent;
using Enums;
using PlayerContent.LevelContent;
using UnityEngine;
using WalletContent;

namespace QuestsContent
{
    public class TaskPrizeRecipient : MonoBehaviour
    {
        [SerializeField] private Wallet _wallet;
        [SerializeField] private Energy _energy;
        [SerializeField] private PlayerLevel _playerLevel;

        public void ClaimPrize(PrizeTask prizeTask)
        {
            switch (prizeTask.TaskPrizeType)
            {
                case TaskPrizeType.Money:
                    _wallet.Add(new DollarValue(prizeTask.Amount, 0));
                    break;
                case TaskPrizeType.XP:
                    _playerLevel.AddExp(prizeTask.Amount);
                    break;
                case TaskPrizeType.Spin:
                    break;
                case TaskPrizeType.Energy:
                    _energy.IncreaseEnergy(prizeTask.Amount);
                    break;
            }
        }
    }
}