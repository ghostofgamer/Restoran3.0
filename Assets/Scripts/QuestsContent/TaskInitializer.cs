using ADSContent;
using AssemblyBurgerContent;
using DayNightContent;
using EnergyContent;
using FortuneContent;
using KitchenEquipmentContent.FryerContent;
using OrdersContent;
using PlayerContent.LevelContent;
using SettingsContent;
using UnityEngine;
using WalletContent;

namespace QuestsContent
{
    public class TaskInitializer : MonoBehaviour
    {
        public static TaskInitializer Instance { get; private set; }
    
        [SerializeField]private Wallet _wallet;
        [SerializeField]private AssemblyBurger _assemblyBurger;
        [SerializeField] private AssemblyFromDeepFry _assemblyFromDeepFry;
        [SerializeField]private LanguageChanger _languageChanger;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Energy _energy;
        [SerializeField] private DayNightCycle _dayNightCycle;
        [SerializeField] private ADS _ads;
        [SerializeField] private Fortune _fortune;
        [SerializeField]private OrdersCounter _ordersCounter;

        public OrdersCounter OrdersCounter => _ordersCounter;
        public Fortune Fortune => _fortune;
        public ADS ADS => _ads;
        public DayNightCycle DayNightCycle => _dayNightCycle;
        public Energy Energy => _energy;
        public PlayerLevel PlayerLevel=>_playerLevel;
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
    }
}