using PlayerContent.LevelContent;
using UnityEngine;
using WalletContent;

namespace StatisticContent
{
    public class StatisticCounter : MonoBehaviour
    {
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Wallet _wallet;

        public int Experience { get; private set; }
        public int Levels { get; private set; }

        public int Income { get; private set; }
        public int Expenses { get; private set; }

        private void OnEnable()
        {
            _playerLevel.ExpAdded += AddExpDay;
            _playerLevel.LevelAdded += AddLevel;
            _wallet.IncomeChanged += AddIncome;
            _wallet.ExpensesChanged += AddExpenses;
        }

        private void OnDisable()
        {
            _playerLevel.ExpAdded -= AddExpDay;
            _playerLevel.LevelAdded -= AddLevel;
            _wallet.IncomeChanged -= AddIncome;
            _wallet.ExpensesChanged -= AddExpenses;
        }

        private void AddExpDay(int exp)
        {
            if (exp <= 0)
                return;

            Experience += exp;
        }

        private void AddLevel()
        {
            Levels++;
        }

        private void AddIncome(int incomeTotalCents)
        {
            Income += incomeTotalCents;
        }

        private void AddExpenses(int expensesTotalCents)
        {
            Expenses += expensesTotalCents;
        }
    }
}