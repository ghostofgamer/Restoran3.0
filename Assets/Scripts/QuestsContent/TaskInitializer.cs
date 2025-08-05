using ADSContent;
using AssemblyBurgerContent;
using DayNightContent;
using EnergyContent;
using Enums;
using FortuneContent;
using KitchenEquipmentContent.FryerContent;
using OrdersContent;
using PlayerContent.LevelContent;
using RestaurantContent.MenuContent;
using SettingsContent;
using UI.Screens.ShopContent;
using UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage;
using UnityEngine;
using WalletContent;
using WorkerContent;

namespace QuestsContent
{
    public class TaskInitializer : MonoBehaviour
    {
        public static TaskInitializer Instance { get; private set; }

        [SerializeField] private Wallet _wallet;
        [SerializeField] private AssemblyBurger _assemblyBurger;
        [SerializeField] private AssemblyFromDeepFry _assemblyFromDeepFry;
        [SerializeField] private LanguageChanger _languageChanger;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Energy _energy;
        [SerializeField] private DayNightCycle _dayNightCycle;
        [SerializeField] private ADS _ads;
        [SerializeField] private Fortune _fortune;
        [SerializeField] private OrdersCounter _ordersCounter;
        [SerializeField] private ItemCartScroll _itemCartScroll;
        [SerializeField] private Workers _workers;
        [SerializeField] private PlacesScrollContent _placesScrollContent;
        [SerializeField] private MenuCounter _menuCounter;
        [SerializeField] private EquipmentScrollContent _equipmentScrollContent;
        [SerializeField] private ZonesScrollContent _zonesScrollContent;

        [Space] [Header("Equipments")] [SerializeField]
        private GameObject _coffeTable;

        [SerializeField] private GameObject _sodaTable;
        [SerializeField] private GameObject _deepFrierTable1;
        [SerializeField] private GameObject[] _shelfs;
        
        [Space] [Header("Zones")] [SerializeField]
        private ZoneUIProduct[] _zoneUIProducts;
        
        public ZonesScrollContent ZonesScrollContent => _zonesScrollContent;
        public EquipmentScrollContent EquipmentScrollContent => _equipmentScrollContent;
        public MenuCounter MenuCounter => _menuCounter;
        public PlacesScrollContent PlacesScrollContent => _placesScrollContent;
        public Workers Workers => _workers;
        public ItemCartScroll ItemCartScroll => _itemCartScroll;
        public OrdersCounter OrdersCounter => _ordersCounter;
        public Fortune Fortune => _fortune;
        public ADS ADS => _ads;
        public DayNightCycle DayNightCycle => _dayNightCycle;
        public Energy Energy => _energy;
        public PlayerLevel PlayerLevel => _playerLevel;
        public Wallet Wallet => _wallet;
        public AssemblyBurger AssemblyBurger => _assemblyBurger;
        public AssemblyFromDeepFry AssemblyFromDeepFry => _assemblyFromDeepFry;
        public LanguageChanger LanguageChanger => _languageChanger;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool GetWorkersUpgradePossibility()
        {
            int value = 0;

            foreach (var worker in _workers.WorkersArray)
            {
                if (worker.MaxLevel > worker.Level && worker.gameObject.activeSelf)
                    value++;
            }

            return value > 0;
        }

        public bool GetBuyPlacesPossibility(int targetValue)
        {
            int value = 0;

            foreach (var placeUIProduct in _placesScrollContent.PlaceUIProductsPayPossibility)
            {
                if (!placeUIProduct.IsOwned)
                    value++;
            }

            return value >= targetValue;
        }
        
        public bool GetCanBuyZone(ZoneType zoneType)
        {
            int value = 0;

            foreach (var zoneUIProduct in _zoneUIProducts)
            {
                if (zoneUIProduct.ZoneType == zoneType)
                    return zoneUIProduct.IsOwned;
            }

            return false;
        }

        public bool GetItemToMenuUsing(ItemType itemType)
        {
            foreach (var itemTypeMenu in _menuCounter.MenuList)
            {
                if (itemType == itemTypeMenu)
                    return true;
            }

            return false;
        }

        public bool GetValueNotPurchasedEquipment(EquipmentType equipmentType)
        {
            switch (equipmentType)
            {
                case EquipmentType.CoffeeTable:
                    return _coffeTable.activeSelf;
                    break;

                case EquipmentType.SodaTable:
                    return _sodaTable.activeSelf;
                    break;

                case EquipmentType.Shelf:
                    int value = 0;

                    foreach (var shelf in _shelfs)
                        if (!shelf.activeSelf)
                            value++;

                    return value <= 0;
                    break;

                case EquipmentType.DeepFryer1:
                    return _deepFrierTable1.activeSelf;
                    break;

                default:
                    return false;
            }
        }
    }
}