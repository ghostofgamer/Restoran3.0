using System.Collections;
using DeliveryContent;
using TMPro;
using UnityEngine;

namespace DeliveryContent
{
    public class DeliveryViewer : MonoBehaviour
    {
        [SerializeField] private GameObject _deliveryScreen;
        [SerializeField] private TMP_Text _timerDeliveryText;
        [SerializeField] private TMP_Text _amountDelivers;
        [SerializeField] private Delivery _delivery;

        private Coroutine _timerCoroutine;
        private float _currentTimer;
        
        private void OnEnable()
        {
            _delivery.AmountItemsDeliveriesChanged += ShowAmountDeliveries;
            
            _delivery.DeliveryTimerStarted += OnStartTimer;
            _delivery.DeliveryTimerStopped += OnStopTimer;
        }

        private void OnDisable()
        {
            _delivery.AmountItemsDeliveriesChanged -= ShowAmountDeliveries;
            
            _delivery.DeliveryTimerStarted -= OnStartTimer;
            _delivery.DeliveryTimerStopped -= OnStopTimer;
            
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
        }

        private void ShowAmountDeliveries(int amount)
        {
            _amountDelivers.text = amount.ToString();
        }
        
        private void OnStartTimer(float duration)
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
            }
        
            _timerCoroutine = StartCoroutine(UpdateTimer(duration));
        }

        private void OnStopTimer()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
            _timerDeliveryText.text = "0:00";
        }

        private IEnumerator UpdateTimer(float duration)
        {
            _currentTimer = duration;
        
            while (_currentTimer > 0)
            {
                _currentTimer -= Time.deltaTime;
                UpdateTimerText();
                yield return null;
            }
        
            _timerDeliveryText.text = "0:00";
        }

        private void UpdateTimerText()
        {
            int seconds = Mathf.CeilToInt(_currentTimer);
            _timerDeliveryText.text = $"{seconds / 60}:{seconds % 60:00}";
        }
    }
}