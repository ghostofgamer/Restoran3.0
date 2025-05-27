using System;
using PlayerContent.LevelContent;
using UnityEngine;

namespace DisableInterContent
{
    public class DisablerInter : MonoBehaviour
    {
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private GameObject _buttonOpenDisableInterScreen;

        private int _currentValueShowReward = 0;

        public event Action<int> CurrentValueChanged;
        
        private void OnEnable()
        {
            _playerLevel.LevelChanged += SetValue;
        }

        private void OnDisable()
        {
            _playerLevel.LevelChanged -= SetValue;
        }

        private void Start()
        {
            SetValue(_playerLevel.CurrentLevel);

            CurrentValueChanged?.Invoke(_currentValueShowReward);
        }

        public void GetReward()
        {
            _currentValueShowReward++;

            if (_currentValueShowReward >= 3)
                _currentValueShowReward = 3;
            
            CurrentValueChanged?.Invoke(_currentValueShowReward);
        }

        private void SetValue(int level)
        {
            _buttonOpenDisableInterScreen.SetActive(level >= 2);
        }
    }
}