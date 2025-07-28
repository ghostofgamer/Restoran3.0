using AssemblyBurgerContent;
using KitchenEquipmentContent.FryerContent;
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
        
        public Wallet Wallet => _wallet;
        public AssemblyBurger AssemblyBurger => _assemblyBurger;
        public AssemblyFromDeepFry AssemblyFromDeepFry => _assemblyFromDeepFry;
        
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