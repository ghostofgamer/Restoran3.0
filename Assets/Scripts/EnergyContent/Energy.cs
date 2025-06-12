using System;
using UI;
using UnityEngine;

namespace EnergyContent
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] private FlyValue _flyValue;
        private int _energyValue;

        public event Action<int> EnergyValueChanged;

        private void Start()
        {
            _energyValue = PlayerPrefs.GetInt("EnergyValue", 0);
            EnergyValueChanged?.Invoke(_energyValue);
        }

        public void IncreaseEnergy(int value)
        {
            if (value <= 0)
                return;
            
            _flyValue.ShowFly(value);
            _energyValue += value;
            SaveEnergy();
            EnergyValueChanged?.Invoke(_energyValue);
        }

        public void DecreaseEnergy(int value)
        {
            _flyValue.ShowFly(-value);
            _energyValue -= value;
            SaveEnergy();
            EnergyValueChanged?.Invoke(_energyValue);
        }

        private void SaveEnergy()
        {
            PlayerPrefs.SetInt("EnergyValue", _energyValue);
        }
    }
}