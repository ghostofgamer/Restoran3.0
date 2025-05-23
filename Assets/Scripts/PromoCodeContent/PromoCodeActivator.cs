using DeliveryContent;
using Enums;
using Io.AppMetrica;
using PlayerContent.LevelContent;
using UnityEngine;
using WalletContent;

namespace PromoCodeContent
{
    public class PromoCodeActivator : MonoBehaviour
    {
        [SerializeField] private Wallet _wallet;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Delivery _delivery;

        public void ActivatePrizePromo()
        {
            AppMetrica.ReportEvent("ActivatePrizePromo");
            _wallet.Add(new DollarValue(350, 0));
            _playerLevel.AddExp(150);
            _delivery.SpawnPrize(ItemType.Bun, 3);
            _delivery.SpawnPrize(ItemType.RawCutlet, 3);
            _delivery.SpawnPrize(ItemType.PackageBurgerPaper, 3);
        }
    }
}