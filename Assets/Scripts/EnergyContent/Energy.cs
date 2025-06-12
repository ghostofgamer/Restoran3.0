using System;
using UnityEngine;

namespace EnergyContent
{
    public class Energy : MonoBehaviour
    {
        private int _energyValue;

        public event Action<int> EnergyValueChanged;

        private void Start()
        {
            _energyValue = PlayerPrefs.GetInt("EnergyValue", 0);
            EnergyValueChanged?.Invoke(_energyValue);
        }

        public void IncreaseEnergy(int value)
        {
            
            SaveEnergy();
            EnergyValueChanged?.Invoke(_energyValue);
        }

        public void DecreaseEnergy(int value)
        {
            
            SaveEnergy();
            EnergyValueChanged?.Invoke(_energyValue);
        }

        private void SaveEnergy()
        {
            PlayerPrefs.SetInt("EnergyValue", _energyValue);
        }
    }
}