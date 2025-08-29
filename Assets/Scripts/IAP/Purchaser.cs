using ADSContent;
using DeliveryContent;
using EnergyContent;
using Enums;
using Io.AppMetrica;
using RestaurantContent;
using SoContent;
using TakeTop.Master;
using UI.Screens.AdsScreens;
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
        [SerializeField] private StoragePackScreen _storagePackScreen;
        [SerializeField] private GameObject _starterPackButton;
        [SerializeField] private GameObject _storagePackButton;
        [SerializeField] private Energy _energy;
        [SerializeField] private ADS _ads;
        [SerializeField] private ZoneWall _storageZoneWall;
        [SerializeField] private ShelfConfigs _shelfConfigs;
        [SerializeField] private GameObject[] _shelfes;

        public void OnPurchaseCompleted(Product product)
        {
            Debug.Log("product " + product);
            
            switch (product.definition.id)
            {
                case "com.serbull.iaptutorial.money100":
                    AddMoney(100,product);
                    break;

                case "com.serbull.iaptutorial.removeads":
                    RemoveAds(product);
                    break;

                case "com.serbull.iaptutorial.money500":
                    AddMoney(500,product);
                    break;

                case "com.serbull.iaptutorial.money1100":
                    AddMoney(1100,product);
                    break;

                case "com.serbull.iaptutorial.money2750":
                    AddMoney(2750,product);
                    break;

                case "com.serbull.iaptutorial.money8000":
                    AddMoney(8000,product);
                    break;

                case "com.serbull.iaptutorial.money20000":
                    AddMoney(20000,product);
                    break;

                case "com.serbull.iaptutorial.starterpack":
                    StarterPack(product);
                    break;

                case "com.serbull.iaptutorial.energy30":
                    AddEnergy(30,product);
                    break;

                case "com.serbull.iaptutorial.energy150":
                    AddEnergy(150,product);
                    break;

                case "com.serbull.iaptutorial.energy450":
                    AddEnergy(450,product);
                    break;

                case "com.serbull.iaptutorial.energy1850":
                    AddEnergy(1850,product);
                    break;

                case "com.serbull.iaptutorial.energy5000":
                    AddEnergy(5000,product);
                    break;

                case "com.serbull.iaptutorial.storagepack":
                    PayStoragePack(product);
                    break;
            }
        }

        [ContextMenu("RemoveAds")]
        private void RemoveAds(Product product)
        {
            PlayerPrefs.SetInt("removeADS", 1);
            Debug.Log("On Purchase RemoveAds Completed");
            AppMetrica.ReportEvent("In_App", "{\"" + "RemoveADS" + "\":null}");

            if (_interstitialTimer != null)
                _interstitialTimer.SetValue(false);

            if (_ads != null)
                _ads.SetValue(false);

            if (_uiInfo != null)
                _uiInfo.UpdateRemoveAdsButton();

            if (_removeAdScreen != null)
                _removeAdScreen.CloseScreen();
            
            SendIapRevenue(product);
        }

        private void StarterPack(Product product)
        {
            PlayerPrefs.SetInt("StarterPack", 1);
            _wallet.Add(new DollarValue(150, 0));
            AppMetrica.ReportEvent("In_App", "{\"" + "StarterPack" + "\":null}");
            _delivery.SpawnPrize(ItemType.Bun, 3);
            _delivery.SpawnPrize(ItemType.RawCutlet, 3);
            _energy.IncreaseEnergy(50);

            Debug.Log("On Purchase StarterPack Completed");

            if (_starterPackScreen != null)
                _starterPackScreen.CloseScreen();

            if (_starterPackButton != null)
                _starterPackButton.SetActive(false);

            SendIapRevenue(product);
        }

        private void AddMoney(int value, Product product)
        {
            _wallet.Add(new DollarValue(value, 0));
            Debug.Log("On Purchase AddMoney Completed");
            SendIapRevenue(product);
        }

        private void AddEnergy(int value, Product product)
        {
            _energy.IncreaseEnergy(value);
            Debug.Log("On Purchase AddEnergy Completed");
            SendIapRevenue(product);
        }

        public void PayStoragePack( Product product)
        {
            PlayerPrefs.SetInt("StoragePack", 1);
            AppMetrica.ReportEvent("In_App", "{\"" + "StoragePack" + "\":null}");
            _wallet.Add(new DollarValue(300, 0));

            _delivery.SpawnPrize(ItemType.Bun, 4);
            _delivery.SpawnPrize(ItemType.RawCutlet, 4);
            _delivery.SpawnPrize(ItemType.PackageBurgerPaper, 4);
            _delivery.SpawnPrize(ItemType.Cheese, 4);
            _delivery.SpawnPrize(ItemType.Coffee, 4);
            _delivery.SpawnPrize(ItemType.CupCoffeeEmpty, 4);
            _delivery.SpawnPrize(ItemType.Tomato, 4);

            int activeShelfs = 0;

            foreach (var shelf in _shelfes)
            {
                if (shelf.activeSelf)
                    activeShelfs++;
            }

            DollarValue amountPrice = new DollarValue(0, 0);

            for (int i = 0; i < activeShelfs; i++)
            {
                amountPrice += _shelfConfigs.shelves[i].price;
                Debug.Log("@ PlusPrice " + _shelfConfigs.shelves[i].price);
            }

            foreach (var shelf in _shelfes)
                shelf.SetActive(true);

            PlayerPrefs.SetInt("ShelfBuyed" + EquipmentType.Shelf, _shelfConfigs.shelves.Length - 1);

            if (PlayerPrefs.GetInt("Zona" + ZoneType.Storage, 0) > 0)
            {
                amountPrice += new DollarValue(100, 0);
            }
            else
            {
                PlayerPrefs.SetInt("Zona" + ZoneType.Storage, 1);
                _storageZoneWall.Activate();
            }

            Debug.Log("@ amountPrice " + amountPrice);
            
            _wallet.Add(new DollarValue(amountPrice.Dollars, 0));
            
            if (_storagePackScreen != null)
                _storagePackScreen.CloseScreen();

            if (_storagePackButton != null)
                _storagePackButton.SetActive(false);
            
            
            SendIapRevenue(product);
        }


        private void SendIapRevenue(Product product)
        {
            IapRevenueData iapRevenueData = new IapRevenueData()
            {
                ProductID = product.definition.id,
                CurrencyCode = product.metadata.isoCurrencyCode,
                LocalizedPrice = (decimal)product.metadata.localizedPrice,
                ProductType = product.definition.type,
            };
            
            Debug.Log("ProductID " + product.definition.id);
            Debug.Log("CurrencyCode " + product.metadata.isoCurrencyCode);
            Debug.Log("LocalizedPrice " + product.metadata.localizedPrice);
            Debug.Log("product.definition.type " + product.definition.type);
            
            TakeTop.Master.Analytics.SendIapRevenue(AnalyticsProviderType.AppMetrica, "payment_succeed", iapRevenueData);
        }
    }
}