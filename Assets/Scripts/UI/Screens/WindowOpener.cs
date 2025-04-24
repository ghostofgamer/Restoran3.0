using KitchenEquipmentContent;
using UI.Screens.AssemblyScreens;
using UnityEngine;

namespace UI.Screens
{
    public class WindowOpener : MonoBehaviour
    {
        [SerializeField] private AssemblyBurgerScreen _assemblyBurgerScreen;
        [SerializeField] private AssemblyCoffeeScreen _assemblyCoffeeScreen;
        [SerializeField] private AssemblySodaScreen _assemblySodaScreen;
        [SerializeField] private AssemblyTable _assemblyTable;
        [SerializeField] private AssemblyDrinkTable _assemblyDrinkTable;
        [SerializeField] private AssemblyDrinkTable _assemblySodaTable;
        
        private void OnEnable()
        {
            _assemblyTable.BurgerAssemblyBeginig += OpenAssemblyBurgerScreen;
            _assemblyDrinkTable.DrinkAssemblyBeginig += OpenAssemblyCoffeeScreen;
            _assemblySodaTable.DrinkAssemblyBeginig += OpenAssemblySodaScreen;
        }

        private void OnDisable()
        {
            _assemblyTable.BurgerAssemblyBeginig -= OpenAssemblyBurgerScreen;
            _assemblyDrinkTable.DrinkAssemblyBeginig -= OpenAssemblyCoffeeScreen;
            _assemblySodaTable.DrinkAssemblyBeginig += OpenAssemblySodaScreen;
        }

        private void OpenAssemblyBurgerScreen()
        {
            _assemblyBurgerScreen.OpenScreen();
        }
        
        private void OpenAssemblyCoffeeScreen()
        {
            _assemblyCoffeeScreen.OpenScreen();
        }
        
        private void OpenAssemblySodaScreen()
        {
            _assemblySodaScreen.OpenScreen();
        }
    }
}
