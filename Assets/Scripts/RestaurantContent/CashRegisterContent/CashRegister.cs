using System;
using System.Collections.Generic;
using System.Linq;
using CameraContent;
using ClientsContent;
using Enums;
using InteractableContent;
using OrdersContent;
using PlayerContent;
using SettingsContent.SoundContent;
using TutorialContent;
using UnityEngine;
using WalletContent;

namespace RestaurantContent.CashRegisterContent
{
    public class CashRegister : MonoBehaviour
    {
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private Restaurant _restaurant;
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private Transform _clientPosition;
        [SerializeField] private GameObject _canvas;
        [SerializeField] private CameraPositionChanger _cameraPositionChanger;
        [SerializeField] private Transform _cameraCurrentPosition;
        [SerializeField] private CashRegisterViewer _cashRegisterViewer;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private PriceOrderCounter _priceOrderCounter;
        
        private Client _currentClient;
        private Coroutine _coroutine;
        private DollarValue _currentGivingValue;
        private DollarValue _currentChangeValue;
        private Stack<int> _changeHistory = new Stack<int>();
        private const int MaxHistorySize = 100;
        
        public event Action<bool> CashRegisterAssemblyBeginig;

        public event Action CashRegisterOrderCompleted;

        public event Action<DollarValue> GivingValueChanged; 

        public Transform ClientPosition => _clientPosition;

        private void OnEnable()
        {
            _interactableObject.OnAction += ShowAssemblyCashRegisterOrder;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= ShowAssemblyCashRegisterOrder;
        }

        public void SetClient(Client client)
        {
            _currentClient = client;
            SetCanvasActive(_currentClient != null);
            _currentChangeValue = _priceOrderCounter.GetChange(client.PriceOrder, client.Cash);
            // _canvas.SetActive(_currentClient != null);
        }

        private void ShowAssemblyCashRegisterOrder(PlayerInteraction playerInteraction)
        {
            SoundPlayer.Instance.PlayButtonClick();
            
            if (playerInteraction.CurrentDraggable != null || playerInteraction.PlayerTray.IsActive)
                return;

            if (_currentClient == null)
                return;
            
            
            /*CashRegisterOrderCompleted?.Invoke();
            _currentClient.Paid();
            _wallet.Add(_currentClient.PriceOrder);
            Client client = _currentClient;
            _currentClient = null;
            SetCanvasActive(_currentClient != null);
            _restaurant.AcceptOrder(client.Order, client);*/
            
            

            CashRegisterAssemblyBeginig?.Invoke(_currentClient.IsCard);
            _cameraPositionChanger.ChangePosition(_cameraCurrentPosition);
            _currentGivingValue = new DollarValue(0, 0);
            _cashRegisterViewer.Init(_currentClient, _currentGivingValue);
        }

        public void SetCanvasActive(bool value)
        {
            _canvas.SetActive(value);
        }

        [ContextMenu("AcceptOrder")]
        public void AcceptOrder()
        {
            if (_currentClient == null)
                return;
            
            if (_currentGivingValue.ToTotalCents() != _currentChangeValue.ToTotalCents())
            {
                SoundPlayer.Instance.PlayError();
                Debug.Log("СДАЧА НЕВЕРНАЯ");
                return;
            }

            if (_tutorial.CurrentType == TutorialType.TakeFirstOrder)
            {
                _tutorial.SetCurrentTutorialStage(TutorialType.TakeFirstOrder);
            }
            
            /*if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartAcceptOrder());*/
            SoundPlayer.Instance.PlayCashRegister();
            CashRegisterOrderCompleted?.Invoke();
            _currentClient.Paid();
            _wallet.Add(_currentClient.PriceOrder);
            Client client = _currentClient;
            _currentClient = null;
            SetCanvasActive(_currentClient != null);
            _restaurant.AcceptOrder(client.Order, client);
            ClearGivingValue();
        }

        public void AcceptCardOrder()
        {
            if (_currentClient == null)
                return;
            
            if (_tutorial.CurrentType == TutorialType.TakeFirstOrder)
                _tutorial.SetCurrentTutorialStage(TutorialType.TakeFirstOrder);
            
            SoundPlayer.Instance.PlayCashRegister();
            CashRegisterOrderCompleted?.Invoke();
            _currentClient.Paid();
            _wallet.Add(_currentClient.PriceOrder);
            Client client = _currentClient;
            _currentClient = null;
            SetCanvasActive(_currentClient != null);
            _restaurant.AcceptOrder(client.Order, client);
            ClearGivingValue();
        }

        public void ChangeGivingValue(int cents)
        {
            int total = _currentGivingValue.ToTotalCents() + cents;
            
            if (total < 0)
                total = 0;
            
            _changeHistory.Push(cents);
            
            if (_changeHistory.Count > MaxHistorySize)
            {
                _changeHistory = new Stack<int>(_changeHistory.ToArray().TakeLast(MaxHistorySize).Reverse());
            }
            
            _currentGivingValue = new DollarValue(0, 0).FromTotalCents(total);
            GivingValueChanged?.Invoke(_currentGivingValue);
        }

        public void ClearGivingValue()
        {
            _currentGivingValue = new DollarValue(0, 0);
            _changeHistory.Clear();
            Debug.Log(_changeHistory.Count);
            GivingValueChanged?.Invoke(_currentGivingValue);
        }
        
        public void UndoLastChange()
        {
            if (_changeHistory.Count > 0)
            {
                int lastChange = _changeHistory.Pop();
                int total = _currentGivingValue.ToTotalCents() - lastChange;
                
                if (total < 0)
                    total = 0;

                _currentGivingValue = new DollarValue(0, 0).FromTotalCents(total);
                GivingValueChanged?.Invoke(_currentGivingValue);
            }
        }

        /*
        private IEnumerator StartAcceptOrder()
        {
            _currentClient.Paid();
            Client client = _currentClient;
            _currentClient = null;
            _canvas.SetActive(_currentClient != null);
            yield return new WaitForSeconds(0.6f);
            _restaurant.AcceptOrder(client.Order, client);
            // _currentClient = null;
        }*/
    }
}