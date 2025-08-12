using CameraContent;
using KitchenEquipmentContent;
using UnityEngine;

namespace UI.Screens.AssemblyScreens
{
    public class AssemblySodaScreen : AbstractScreen
    {
        [SerializeField] private GameObject _input;
        [SerializeField] private CameraPositionChanger _cameraPositionChanger;
        [SerializeField] private AssemblyDrinkTable _assemblyTable;
        [SerializeField] private GameObject _buttonsContentTopUI;
        
        public override void OpenScreen()
        {
            Debug.Log("OPENASSEMBLYSCREEN");
            base.OpenScreen();
            _input.SetActive(false);
            _buttonsContentTopUI.SetActive(false);
        }

        public override void CloseScreen()
        {
            _cameraPositionChanger.ReturnDefaultPosition();
            base.CloseScreen();
            _assemblyTable.SetValueCollider(true);
            _input.SetActive(true);
            _buttonsContentTopUI.SetActive(true);
        }
    }
}