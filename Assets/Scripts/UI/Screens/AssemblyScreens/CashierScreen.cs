using System;
using CameraContent;
using Enums;
using RestaurantContent.CashRegisterContent;
using TutorialContent;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.AssemblyScreens
{
    public class CashierScreen : AbstractScreen
    {
        [SerializeField] private GameObject _input;
        [SerializeField] private CameraPositionChanger _cameraPositionChanger;
        [SerializeField] private CashRegisterViewer _cashRegisterViewer;
        [SerializeField] private CashRegister _cashRegister;
        [SerializeField] private Button _closeButton;
        [SerializeField]private Tutorial _tutorial;
        [SerializeField] private GameObject _tutorTouchAnim;

        public event Action CloseCashierScreens;
        
        public override void OpenScreen()
        {
            Debug.Log("OPENASSEMBLYSCREEN");
            base.OpenScreen();
            _input.SetActive(false);

            /*if (_tutorial.CurrentType == TutorialType.TakeFirstOrder)
                SetCloseButtonValue(false);*/
        }

        public override void CloseScreen()
        {
            CloseCashierScreens?.Invoke();
            _cameraPositionChanger.ReturnDefaultPosition();
            base.CloseScreen();
            _input.SetActive(true);
            _cashRegister.SetPlayerValue(false);

            /*if (_tutorial.CurrentType == TutorialType.TakeFirstOrder)
            {
                _tutorTouchAnim.SetActive(false);
                _tutorial.SetCurrentTutorialStage(TutorialType.TakeFirstOrder);
            }*/
        }

        public void SetCloseButtonValue(bool value)
        {
            _closeButton.interactable = value;
            _tutorTouchAnim.SetActive(value);
        }
    }
}