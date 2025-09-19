using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ADSContent;
using CustomizationContent.InapButtonContent;
using DeliveryContent;
using EnergyContent;
using Enums;
using Io.AppMetrica;
using RestaurantContent;
using SoContent;
using TakeTop.Master;
using UI.Screens.AdsScreens;
using UI.Screens.ShopContent.ShopPages.PageContents.StylePage.UIStyleItemContent.StyleContent;
using UnityEngine;
using UnityEngine.Purchasing;
using WalletContent;
using Product = UnityEngine.Purchasing.Product;

namespace IAP
{
    public class Purchaser : MonoBehaviour
    {
        private StoreController _storeController;

        private readonly List<ProductDefinition> _productsToFetch = new List<ProductDefinition>
        {
            new ProductDefinition(IapIds.GetId(IapProductType.Money100), ProductType.Consumable),
            new ProductDefinition(IapIds.GetId(IapProductType.Money500), ProductType.Consumable),
            new ProductDefinition(IapIds.GetId(IapProductType.Money1100), ProductType.Consumable),
            new ProductDefinition(IapIds.GetId(IapProductType.Money2750), ProductType.Consumable),
            new ProductDefinition(IapIds.GetId(IapProductType.Money8000), ProductType.Consumable),
            new ProductDefinition(IapIds.GetId(IapProductType.Money20000), ProductType.Consumable),
            new ProductDefinition(IapIds.GetId(IapProductType.RemoveAds), ProductType.NonConsumable),
            new ProductDefinition(IapIds.GetId(IapProductType.StarterPack), ProductType.NonConsumable),
            new ProductDefinition(IapIds.GetId(IapProductType.Energy30), ProductType.Consumable),
            new ProductDefinition(IapIds.GetId(IapProductType.Energy150), ProductType.Consumable),
            new ProductDefinition(IapIds.GetId(IapProductType.Energy450), ProductType.Consumable),
            new ProductDefinition(IapIds.GetId(IapProductType.Energy1850), ProductType.Consumable),
            new ProductDefinition(IapIds.GetId(IapProductType.Energy5000), ProductType.Consumable),
            new ProductDefinition(IapIds.GetId(IapProductType.StoragePack), ProductType.NonConsumable),
        };

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
        [SerializeField] private StyleUIElement[] _styleUIElements;
        [SerializeField] private StyleUIElement[] _styleFurnitureUIElements;
        [SerializeField] private StyleUIElement[] _styleDecorUIElements;
        [SerializeField] private StyleInapElements[] _styleInapElements;
        [SerializeField] private StyleInapElements[] _styleFurnitureInapElements;
        [SerializeField] private StyleInapElements[] _styleDecorInapElements;

        public event Action RemoveADSPurchased;

        private string ReceiptsFilePath => Path.Combine(Application.persistentDataPath, "processed_receipts.json");

        private void Awake()
        {
            InitializeIAP();
        }

        public void RestorePurchases()
        {
#if UNITY_ANDROID
            // Для Android и других платформ просто вызываем FetchPurchases()
            if (_storeController != null)
            {
                _storeController.FetchPurchases();
                Debug.Log("Android/Other Restore triggered via FetchPurchases()");
            }
#endif
        }

        public void OnPurchaseCompleted(Product product)
        {
            if (product == null) return;

            Debug.Log("!!!!!!!!!!!!!!!!product " + product);

            switch (product.definition.id)
            {
                case "com.serbull.iaptutorial.money100":
                    AddMoney(100, product);
                    break;

                case "com.serbull.iaptutorial.removeads":
                    RemoveAds(product);
                    break;

                case "com.serbull.iaptutorial.money500":
                    AddMoney(500, product);
                    break;

                case "com.serbull.iaptutorial.money1100":
                    AddMoney(1100, product);
                    break;

                case "com.serbull.iaptutorial.money2750":
                    AddMoney(2750, product);
                    break;

                case "com.serbull.iaptutorial.money8000":
                    AddMoney(8000, product);
                    break;

                case "com.serbull.iaptutorial.money20000":
                    AddMoney(20000, product);
                    break;

                case "com.serbull.iaptutorial.starterpack":
                    StarterPack(product);
                    break;

                case "com.serbull.iaptutorial.energy30":
                    AddEnergy(30, product);
                    break;

                case "com.serbull.iaptutorial.energy150":
                    AddEnergy(150, product);
                    break;

                case "com.serbull.iaptutorial.energy450":
                    AddEnergy(450, product);
                    break;

                case "com.serbull.iaptutorial.energy1850":
                    AddEnergy(1850, product);
                    break;

                case "com.serbull.iaptutorial.energy5000":
                    AddEnergy(5000, product);
                    break;

                case "com.serbull.iaptutorial.storagepack":
                    PayStoragePack(product);
                    break;

                case "com.serbull.iaptutorial.stylepack":
                {
                    Debug.Log("On Purchase StylePack Completed");
                    StylePack(product);
                }
                    break;

                case "com.serbull.iaptutorial.stylefurniturepack":
                {
                    Debug.Log("On Purchase StyleFurniturePack Completed");
                    StyleFurniturePack(product);
                }
                    break;

                case "com.serbull.iaptutorial.styledecorpack":
                {
                    Debug.Log("On Purchase StyleFurniturePack Completed");
                    StyleDecorPack(product);
                }
                    break;
            }
        }

        [ContextMenu("RemoveAds")]
        private void RemoveAds(Product product)
        {
            PlayerPrefs.SetInt("removeADS", 1);
            Debug.Log("On Purchase RemoveAds Completed");
            AppMetrica.ReportEvent("In_App", "{\"" + "RemoveADS" + "\":null}");

            RemoveADSPurchased?.Invoke();

            if (_interstitialTimer != null)
                _interstitialTimer.SetValue(false);

            if (_ads != null)
                _ads.SetValue(false);

            if (_uiInfo != null)
                _uiInfo.UpdateRemoveAdsButton();

            if (_removeAdScreen != null)
                _removeAdScreen.CloseScreen();

            bool isRestore = CheckReceiptInLocalJSON(product.receipt);

            if (!isRestore)
            {
                Debug.Log("!isRestore  / PurchsedComplitedFirst ");
                SaveReceiptInLocalJSON(product.receipt);
                SendIapRevenue(product);
            }
            else
            {
                Debug.Log("Restore detected — revenue not sent");
            }
            // SendIapRevenue(product);
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

            bool isRestore = CheckReceiptInLocalJSON(product.receipt);

            if (!isRestore)
            {
                Debug.Log("!isRestore  / PurchsedComplitedFirst ");
                SaveReceiptInLocalJSON(product.receipt);
                SendIapRevenue(product);
            }
            else
            {
                Debug.Log("Restore detected — revenue not sent");
            }
            // SendIapRevenue(product);
        }

        private void StylePack(Product product)
        {
            PlayerPrefs.SetInt("StylePack", 1);
            AppMetrica.ReportEvent("In_App", "{\"" + "StylePack" + "\":null}");

            Debug.Log("On Purchase StylePack Completed");

            int value = 0;
            foreach (var styleUIElement in _styleUIElements)
            {
                styleUIElement.Purchase();

                Debug.Log("value" + value);
                value++;
            }

            foreach (var styleInapElements in _styleInapElements)
                styleInapElements.Deactivate();
        }

        private void StyleFurniturePack(Product product)
        {
            PlayerPrefs.SetInt("StyleFurniturePack", 1);
            AppMetrica.ReportEvent("In_App", "{\"" + "StyleFurniturePack" + "\":null}");

            Debug.Log("On Purchase StyleFurniturePack Completed");

            int value = 0;
            foreach (var styleUIElement in _styleFurnitureUIElements)
            {
                styleUIElement.Purchase();

                Debug.Log("value" + value);
                value++;
            }

            foreach (var styleInapElements in _styleFurnitureInapElements)
                styleInapElements.Deactivate();
        }

        private void StyleDecorPack(Product product)
        {
            PlayerPrefs.SetInt("StyleDecorPack", 1);
            AppMetrica.ReportEvent("In_App", "{\"" + "StyleDecorPack" + "\":null}");

            Debug.Log("On Purchase StyleDecorPack Completed");

            int value = 0;
            foreach (var styleUIElement in _styleDecorUIElements)
            {
                styleUIElement.Purchase();

                Debug.Log("value" + value);
                value++;
            }

            foreach (var styleInapElements in _styleDecorInapElements)
                styleInapElements.Deactivate();
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

        public void PayStoragePack(Product product)
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

            bool isRestore = CheckReceiptInLocalJSON(product.receipt);

            if (!isRestore)
            {
                Debug.Log("!isRestore  / PurchsedComplitedFirst ");
                SaveReceiptInLocalJSON(product.receipt);
                SendIapRevenue(product);
            }
            else
            {
                Debug.Log("Restore detected — revenue not sent");
            }
            // SendIapRevenue(product);
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
            Debug.Log("LocalizedPrice " + (decimal)product.metadata.localizedPrice);
            Debug.Log("product.definition.type " + product.definition.type);

            TakeTop.Master.Analytics.SendIapRevenue(AnalyticsProviderType.AppMetrica, "payment_succeed",
                iapRevenueData);
        }

        private ProcessedReceiptsData LoadReceipts()
        {
            if (!File.Exists(ReceiptsFilePath)) return new ProcessedReceiptsData();
            string json = File.ReadAllText(ReceiptsFilePath);
            return JsonUtility.FromJson<ProcessedReceiptsData>(json);
        }

        private void SaveReceipts(ProcessedReceiptsData data)
        {
            File.WriteAllText(ReceiptsFilePath, JsonUtility.ToJson(data));
        }

        private bool CheckReceiptInLocalJSON(string receipt)
        {
            if (string.IsNullOrEmpty(receipt)) return false;
            return LoadReceipts().receipts.Contains(receipt);
        }

        private void SaveReceiptInLocalJSON(string receipt)
        {
            if (string.IsNullOrEmpty(receipt)) return;
            var data = LoadReceipts();
            if (!data.receipts.Contains(receipt))
            {
                data.receipts.Add(receipt);
                SaveReceipts(data);
            }
        }

        private async void InitializeIAP()
        {
            _storeController = UnityIAPServices.StoreController();

            _storeController.OnPurchasePending += OnPurchasePending;
            _storeController.OnPurchaseConfirmed += OnPurchaseConfirmed;
            _storeController.OnPurchaseFailed += OnPurchaseFailed;
            _storeController.OnStoreDisconnected += OnStoreDisconnected;
            _storeController.OnProductsFetched += OnProductsFetched;
            _storeController.OnProductsFetchFailed += OnProductsFetchedFailed;

            Debug.Log("Connecting to store...");
            await _storeController.Connect();

            _storeController.FetchProducts(_productsToFetch);
        }

        public void BuyProduct(IapProductType productType)
        {
            string productId = IapIds.GetId(productType);
            if (_storeController != null)
                _storeController.PurchaseProduct(productId);
            else
                Debug.LogWarning("StoreController is not initialized yet.");
        }

        private void OnPurchasePending(PendingOrder order)
        {
            var product = order.CartOrdered.Items().FirstOrDefault()?.Product;
            if (product == null) return;

            Debug.Log("!!!!!!!!!!!!!!!!!!!!Purchase pending: " + product.definition.id);

            OnPurchaseCompleted(product);
            _storeController.ConfirmPurchase(order);
        }

        void OnPurchaseConfirmed(Order order)
        {
            switch (order)
            {
                case ConfirmedOrder confirmedOrder:
                    OnPurchaseConfirmed(confirmedOrder);
                    break;
                case FailedOrder failedOrder:
                    OnPurchaseConfirmationFailed(failedOrder);
                    break;
                default:
                    Debug.Log("Unknown OnPurchaseConfirmed result.");
                    break;
            }
        }

        void OnPurchaseConfirmationFailed(FailedOrder order)
        {
            var product = GetFirstProductInOrder(order);
            if (product == null)
            {
                Debug.Log("Could not find product in failed confirmation.");
            }

            Debug.Log($"Confirmation failed - Product: '{product?.definition.id}'," +
                      $"PurchaseFailureReason: {order.FailureReason.ToString()},"
                      + $"Confirmation Failure Details: {order.Details}");
        }

        void OnPurchaseConfirmed(ConfirmedOrder order)
        {
            var product = GetFirstProductInOrder(order);
            Debug.Log($"Purchase confirmed (for log only) - Product: {product?.definition.id}");
            // Здесь больше **не начисляем**, только логируем и при необходимости аналитика
        }

        Product GetFirstProductInOrder(Order order)
        {
            return order.CartOrdered.Items().First()?.Product;
        }


        private void OnPurchaseFailed(FailedOrder order)
        {
            var product = order.CartOrdered.Items().FirstOrDefault()?.Product;
            Debug.LogWarning(
                $"Purchase failed: {product?.definition.id}, Reason: {order.FailureReason}, Details: {order.Details}");
        }

        private void OnProductsFetched(List<Product> products)
        {
            Debug.Log($"Products fetched successfully: {products.Count}");
        }

        private void OnProductsFetchedFailed(ProductFetchFailed failure)
        {
            Debug.LogWarning(
                $"Products fetch failed: {failure.FailedFetchProducts.Count} items, Reason: {failure.FailureReason}");
        }

        private void OnStoreDisconnected(StoreConnectionFailureDescription description)
        {
            Debug.LogWarning("Store disconnected: " + description.message);
        }
    }

    [System.Serializable]
    public class ProcessedReceiptsData
    {
        public List<string> receipts = new List<string>();
    }
}