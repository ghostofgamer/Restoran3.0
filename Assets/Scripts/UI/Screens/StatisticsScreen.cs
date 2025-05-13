using System;
using StatisticContent;
using TMPro;
using UnityEngine;
using WalletContent;

namespace UI.Screens
{
    public class StatisticsScreen : AbstractScreen
    {
        [SerializeField] private StatisticCounter _statisticCounter;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private TMP_Text _experienceText;
        [SerializeField] private TMP_Text _levelsText;
        [SerializeField] private TMP_Text _incomeText;
        [SerializeField] private TMP_Text _expensesText;
        [SerializeField] private TMP_Text _profitText;
        [SerializeField] private TMP_Text _balanceText;

        public void ShowStatistic()
        {
            _experienceText.text = $"Experience: {_statisticCounter.Experience}";
            _levelsText.text = $"Levels: +{_statisticCounter.Levels}";
            _incomeText.text = $"Income: {new DollarValue(0,0).FromTotalCents(_statisticCounter.Income)}";
            _expensesText.text = $"Expenses: <color=#FF0000>-{new DollarValue(0,0).FromTotalCents(_statisticCounter.Expenses)}</color>";
            
            int profitCents = _statisticCounter.Income - _statisticCounter.Expenses; 
            bool isProfitNegative = profitCents < 0;
            int absProfitCents = Math.Abs(profitCents);
            string profitText = new DollarValue(0, 0).FromTotalCents(absProfitCents).ToString();
            string sign = isProfitNegative ? "-" : "+";
            string colorTag = isProfitNegative ? "<color=#FF0000>" : "<color=#00FF00>";
            string endColorTag = "</color>";
            
            _profitText.text = $"Profit: {colorTag}{sign}{profitText}{endColorTag}";
            _balanceText.text = $"Balance: {_wallet.DollarValue}";
        }
    }
}