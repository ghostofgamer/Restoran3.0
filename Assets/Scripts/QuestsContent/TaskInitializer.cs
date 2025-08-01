using AssemblyBurgerContent;
using KitchenEquipmentContent.FryerContent;
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