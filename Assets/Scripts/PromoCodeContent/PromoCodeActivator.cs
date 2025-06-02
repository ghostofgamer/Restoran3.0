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
            _wallet.Add(new DollarValue(50, 0));
            _delivery.SpawnPrize(ItemType.Nuggets, 1);
            _delivery.SpawnPrize(ItemType.FrenchFries, 1);
            _delivery.SpawnPrize(ItemType.FrenchFriesPackage, 1);
            _delivery.SpawnPrize(ItemType.NuggetsPackage, 1);
        }
    }
}