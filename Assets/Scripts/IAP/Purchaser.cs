using DeliveryContent;
using Enums;
using UnityEngine;
using UnityEngine.Purchasing;
using WalletContent;

namespace IAP
{
    public class Purchaser : MonoBehaviour
    {
        [SerializeField] private UIInfo _uiInfo;
        [SerializeField] private Wallet _wallet;

        private Delivery _delivery;

        public void OnPurchaseCompleted(Product product)
        {
            switch (product.definition.id)
            {
                case "com.serbull.iaptutorial.money100":
                    AddMoney(100);
                    break;

                case "com.serbull.iaptutorial.removeads":
                    RemoveAds();
                    break;

                case "com.serbull.iaptutorial.money500":
                    AddMoney(500);
                    break;

                case "com.serbull.iaptutorial.money1100":
                    AddMoney(1100);
                    break;

                case "com.serbull.iaptutorial.money2750":
                    AddMoney(2750);
                    break;

                case "com.serbull.iaptutorial.money8000":
                    AddMoney(8000);
                    break;

                case "com.serbull.iaptutorial.money20000":
                    AddMoney(20000);
                    break;

                case "com.serbull.iaptutorial.starterpack":
                    StarterPack();
                    break;
            }
        }

        private void RemoveAds()
        {
            PlayerPrefs.SetInt("removeADS", 1);
            Debug.Log("On Purchase RemoveAds Completed");

            /*if (_gui != null)
                _gui.TopUiContainer.DeactivateRemoveInterPurchase();*/

            if (_uiInfo != null)
                _uiInfo.UpdateRemoveAdsButton();
        }

        private void StarterPack()
        {
            PlayerPrefs.SetInt("StarterPack", 1);
            AddMoney(150);

            _delivery.SpawnPrize(ItemType.Bun,3);
            _delivery.SpawnPrize(ItemType.RawCutlet,3);

            Debug.Log("On Purchase StarterPack Completed");
        }

        private void AddMoney(int value)
        {
            _wallet.Add(new DollarValue(value,0));
            Debug.Log("On Purchase AddMoney Completed");
        }
       
    }
}