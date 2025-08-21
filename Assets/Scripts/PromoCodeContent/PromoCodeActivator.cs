using System;
using DeliveryContent;
using EnergyContent;
using Enums;
using Io.AppMetrica;
using PlayerContent.LevelContent;
using UnityEngine;
using WalletContent;

namespace PromoCodeContent
{
    public class PromoCodeActivator : MonoBehaviour
    {
        [SerializeField] private PromoCodePrize[] _promoCodePrizes;
        
        [SerializeField] private Wallet _wallet;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Delivery _delivery;
        [SerializeField] private Energy _energy;

        /*public void ActivatePrizePromo()
        {
            AppMetrica.ReportEvent("ActivatePrizePromo");
            _wallet.Add(new DollarValue(50, 0));
            _delivery.SpawnPrize(ItemType.Cabbage, 2);
            _delivery.SpawnPrize(ItemType.Coffee, 2);
        }*/
        
        public void ActivatePrizePromo(PromoCodesType promoCodeType)
        {
            AppMetrica.ReportEvent("ActivatePrizePromo", "{\"" + promoCodeType.ToString() + "\":null}");

            foreach (var prize in _promoCodePrizes)
            {
                if (prize.PromoCodeType == promoCodeType)
                {
                    if (prize.MoneyReward > 0)
                        _wallet.Add(new DollarValue(prize.MoneyReward, 0));

                    if (prize.PrizeItems != null)
                    {
                        foreach (var item in prize.PrizeItems)
                            _delivery.SpawnPrize(item.ItemType, item.Amount);
                    }
                    return;
                }
            }
            Debug.LogError($"No prize configured for promo code: {promoCodeType}");
        }
    }
    
    [Serializable]
    public class PromoCodePrize
    {
        public PromoCodesType PromoCodeType;
        public int MoneyReward;
        public PromoPrizeItem[] PrizeItems;
    }
    
    [Serializable]
    public struct PromoPrizeItem
    {
        public ItemType ItemType;
        public int Amount;
    }
}