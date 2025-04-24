using KitchenEquipmentContent;
using UnityEngine;

namespace UI.Screens
{
    public class WindowOpener : MonoBehaviour
    {
        [SerializeField] private AssemblyBurgerScreen _assemblyBurgerScreen;
        [SerializeField] private AssemblyCoffeeScreen _assemblyCoffeeScreen;
        [SerializeField] private AssemblyTable _assemblyTable;
        [SerializeField] private AssemblyDrinkTable _assemblyDrinkTable;
        
        private void OnEnable()
        {
            _assemblyTable.BurgerAssemblyBeginig += OpenAssemblyBurgerScreen;
            _assemblyDrinkTable.DrinkAssemblyBeginig += OpenAssemblyCoffeeScreen;
        }

        private void OnDisable()
        {
            _assemblyTable.BurgerAssemblyBeginig -= OpenAssemblyBurgerScreen;
            _assemblyDrinkTable.DrinkAssemblyBeginig -= OpenAssemblyCoffeeScreen;
        }

        private void OpenAssemblyBurgerScreen()
        {
            _assemblyBurgerScreen.OpenScreen();
        }
        
        private void OpenAssemblyCoffeeScreen()
        {
            _assemblyCoffeeScreen.OpenScreen();
        }
    }
}
