using System;
using CameraContent;
using RestaurantContent.CashRegisterContent;
using UnityEngine;

namespace UI.Screens.AssemblyScreens
{
    public class CashierScreen : AbstractScreen
    {
        [SerializeField] private GameObject _input;
        [SerializeField] private CameraPositionChanger _cameraPositionChanger;
        [SerializeField] private CashRegisterViewer _cashRegisterViewer;
        [SerializeField] private CashRegister _cashRegister;

        public event Action CloseCashierScreens;
        
        public override void OpenScreen()
        {
            Debug.Log("OPENASSEMBLYSCREEN");
            base.OpenScreen();
            _input.SetActive(false);
        }

        public override void CloseScreen()
        {
            CloseCashierScreens?.Invoke();
            _cameraPositionChanger.ReturnDefaultPosition();
            base.CloseScreen();
            _input.SetActive(true);
            _cashRegister.SetPlayerValue(false);
        }
    }
}