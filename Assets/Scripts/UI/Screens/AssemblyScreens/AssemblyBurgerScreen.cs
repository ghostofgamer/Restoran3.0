using CameraContent;
using UnityEngine;

namespace UI.Screens
{
    public class AssemblyBurgerScreen : AbstractScreen
    {
        [SerializeField] private GameObject _input;
        [SerializeField] private CameraPositionChanger _cameraPositionChanger;
        [SerializeField] private AssemblyTable _assemblyTable;
        [SerializeField] private GameObject _buttonsContentTopUI;
        [SerializeField] private GameObject[] _deactivateContent;
        
        public override void OpenScreen()
        {
            Debug.Log("OPENASSEMBLYSCREEN");
            base.OpenScreen();
            _input.SetActive(false);
            _buttonsContentTopUI.SetActive(false);
            SetValue(false);
        }

        public override void CloseScreen()
        {
            _cameraPositionChanger.ReturnDefaultPosition();
            base.CloseScreen();
            _assemblyTable.SetValueCollider(true);
            _input.SetActive(true);
            _buttonsContentTopUI.SetActive(true);
            SetValue(true);
        }

        private void SetValue(bool value)
        {
            foreach (var deactivateObject in _deactivateContent)
                deactivateObject.SetActive(value);
        }
    }
}