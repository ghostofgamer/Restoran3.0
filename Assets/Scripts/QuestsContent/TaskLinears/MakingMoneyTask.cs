using UnityEngine;
using WalletContent;

namespace QuestsContent.TaskLinears
{
    [CreateAssetMenu(fileName = "MakingMoneyTask", menuName = "QuestConfigs/MakingMoneyTaskConfig", order = 1)]
    public class MakingMoneyTask : Task
    {
        [SerializeField] private int _targetValue;

        private Wallet _wallet;
        private int _currentValue;

        public override bool CheckCompletion()
        {
            Debug.Log("CheckCompletionTask");
            return false;
        }

        protected override void Initialization()
        {
            Debug.Log("InitializationTask");
            _wallet = TaskInitializer.Instance.Wallet;
            _currentValue = 0;
        }

        protected override void SubscribeToEvents()
        {
            Debug.Log("SubscribeToEventsTask");
            _wallet.IncomeChanged += ChangeValue;
        }

        public override void UnsubscribeFromEvents()
        {
            Debug.Log("UnsubscribeFromEventsTask");
            _wallet.IncomeChanged -= ChangeValue;
        }

        private void ChangeValue(int value)
        {
            DollarValue walletValue = new DollarValue(0,0).FromTotalCents(value);
            
            _currentValue += walletValue.Dollars;
            Debug.Log("ChangeValueTask " + _currentValue);
            Debug.Log("_targetValue " + _targetValue);

            if (_currentValue >= _targetValue)
                CompleteTask();
        }
    }
}