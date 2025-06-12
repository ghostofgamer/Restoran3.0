using ADSContent;
using DeliveryContent;
using EnergyContent;
using Enums;
using UI.Screens.AdsScreens;
using Unity.VisualScripting;
using UnityEngine;
using WalletContent;
using Product = UnityEngine.Purchasing.Product;

namespace IAP
{
    public class Purchaser : MonoBehaviour
    {
        [SerializeField] private UIInfo _uiInfo;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private InterstitialTimer _interstitialTimer;
        [SerializeField] private Delivery _delivery;
        [SerializeField] private RemoveAdScreen _removeAdScreen;
        [SerializeField] private StarterPackScreen _starterPackScreen;
        [SerializeField] private GameObject _starterPackButton;
        [SerializeField] private Energy _energy;

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

                case "com.serbull.iaptutorial.energy30":
                    AddEnergy(30);
                    break;
                
                case "com.serbull.iaptutorial.energy150":
                    AddEnergy(150);
                    break;
                
                case "com.serbull.iaptutorial.energy450":
                    AddEnergy(450);
                    break;
                
                case "com.serbull.iaptutorial.energy1850":
                    AddEnergy(1850);
                    break;
                
                case "com.serbull.iaptutorial.energy5000":
                    AddEnergy(5000);
                    break;
            }
        }

        private void RemoveAds()
        {
            PlayerPrefs.SetInt("removeADS", 1);
            Debug.Log("On Purchase RemoveAds Completed");

            if (_interstitialTimer != null)
                _interstitialTimer.SetValue(false);

            if (_uiInfo != null)
                _uiInfo.UpdateRemoveAdsButton();

            if (_removeAdScreen != null)
                _removeAdScreen.CloseScreen();
        }

        private void StarterPack()
        {
            PlayerPrefs.SetInt("StarterPack", 1);
            AddMoney(150);

            _delivery.SpawnPrize(ItemType.Bun, 3);
            _delivery.SpawnPrize(ItemType.RawCutlet, 3);

            Debug.Log("On Purchase StarterPack Completed");

            if (_starterPackScreen != null)
                _starterPackScreen.CloseScreen();

            if (_starterPackButton != null)
                _starterPackButton.SetActive(false);
        }

        private void AddMoney(int value)
        {
            _wallet.Add(new DollarValue(value, 0));
            Debug.Log("On Purchase AddMoney Completed");
        }

        private void AddEnergy(int value)
        {
            _energy.IncreaseEnergy(value);
            Debug.Log("On Purchase AddEnergy Completed");
        }
    }
}