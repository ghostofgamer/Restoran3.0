using CameraContent;
using UnityEngine;

namespace UI.Screens
{
    public class WindowOpener : MonoBehaviour
    {
        [SerializeField] private AssemblyBurgerScreen _assemblyBurgerScreen;
        [SerializeField] private AssemblyTable _assemblyTable;
        
        private void OnEnable()
        {
            _assemblyTable.BurgerAssemblyBeginig += OpenAssemblyBurgerScreen;
        }

        private void OnDisable()
        {
            _assemblyTable.BurgerAssemblyBeginig -= OpenAssemblyBurgerScreen;
        }

        private void OpenAssemblyBurgerScreen()
        {
            _assemblyBurgerScreen.OpenScreen();
        }
    }
}
