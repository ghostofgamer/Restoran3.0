using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerContent.LevelContent
{
    public class PlayerLevel : MonoBehaviour
    {
        public List<LevelConfig> levelConfigs;
        
        private int _minLevel = 1;
        private int _currentExp;
        private int _targetExp;

        public event Action<int> LevelChanged;
        public event Action<int, int> ExpChanged;

        public int CurrentLevel { get; private set; }
        
        private void Start()
        {
            CurrentLevel = PlayerPrefs.GetInt("Level", _minLevel);
            _currentExp = PlayerPrefs.GetInt("Exp", 0);
            _targetExp = GetExpForLevel(CurrentLevel);
            Debug.Log("_targetStartExp " + _targetExp);
            LevelChanged?.Invoke(CurrentLevel);
            ExpChanged?.Invoke(_currentExp, _targetExp);
        }

        [ContextMenu("TestAddCurrentExp")]
        public void TestAddExp()
        {
            AddExp(563);
        }

        public void AddExp(int valueExp)
        {
            if (valueExp <= 0)
                return;

            _currentExp += valueExp;

            /*if (_currentExp >= _targetExp)
            {
                LevelUp();
            }*/
            
            while (_currentExp >= _targetExp && CurrentLevel < levelConfigs.Count)
            {
                LevelUp();
            }
            
            PlayerPrefs.SetInt("Exp", _currentExp);
            ExpChanged?.Invoke(_currentExp, _targetExp);
        }

        private void LevelUp()
        {
            CurrentLevel++;
            PlayerPrefs.SetInt("Level", CurrentLevel);
            _targetExp = GetExpForLevel(CurrentLevel);
            Debug.Log("_targetExp " + _targetExp);
            LevelChanged?.Invoke(CurrentLevel);
        }
        
        private int GetExpForLevel(int level)
        {
            foreach (var config in levelConfigs)
            {
                if (config.level == level)
                {
                    return config.expRequired;
                }
            }
            return int.MaxValue;
        }
    }
}