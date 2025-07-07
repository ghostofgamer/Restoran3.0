using System;
using UnityEngine;
using WalletContent;

namespace SoContent
{
    [CreateAssetMenu(fileName = "NewWorkerParametersConfig", menuName = "Configs/WorkerParametersConfig")]
    public class WorkerParametersConfig : ScriptableObject
    {
        [SerializeField] private WorkerParameterConfig[] _workerParameterConfigs;

        public WorkerParameterConfig GetConfig(int level)
        {
            foreach (WorkerParameterConfig workerParameterConfig in _workerParameterConfigs)
            {
                if (workerParameterConfig.Level == level)
                    return workerParameterConfig;
            }

            Debug.LogError($"Level {level} config not found.");
            return null;
        }
    }

    [Serializable]
    public class WorkerParameterConfig
    {
        [SerializeField] private int _level;
        [SerializeField] private float _delayWork;
        [SerializeField] private float _delayWorkNext;
        [SerializeField] private float _delayRelax;
        [SerializeField] private float _speed;
        [SerializeField] private float _efficiency;
        [SerializeField] private DollarValue _priceUpgrade;

        public int MaxLevel { get; private set; } = 6;

        public int Level => _level;

        public float DelayWork => _delayWork;
        public float DelayWorkNext => _delayWorkNext;

        public float DelayRelax => _delayRelax;

        public float Speed => _speed;

        public float Efficiency => _efficiency;

        public DollarValue PriceUpgrade => _priceUpgrade;
    }
}