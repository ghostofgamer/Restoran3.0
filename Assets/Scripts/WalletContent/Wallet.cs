using System;
using UI;
using UnityEngine;

namespace WalletContent
{
    public class Wallet : MonoBehaviour
    {
        [SerializeField] private FlyValue _flyValue;
        
        public DollarValue DollarValue { get; private set; }

        public event Action<DollarValue> DollarValueChanged;

        public event Action<int> IncomeChanged;
        public event Action<int> ExpensesChanged;

        private void Start()
        {
            DollarValue = new DollarValue(100, 10);
            DollarValueChanged.Invoke(DollarValue);
        }

        public void AddTest()
        {
            Add(new DollarValue(1003, 65));
        }

        public void SubtractTest()
        {
            Subtract(new DollarValue(135, 06));
        }

        public void Add(DollarValue other)
        {
            int totalCents = ToTotalCents(DollarValue) + ToTotalCents(other);
            DollarValue = FromTotalCents(totalCents);
            DollarValueChanged.Invoke(DollarValue);
            _flyValue.ShowFly(other,true);
            IncomeChanged?.Invoke(ToTotalCents(other));
        }

        public void Subtract(DollarValue other)
        {
            int totalCents = ToTotalCents(DollarValue) - ToTotalCents(other);
            DollarValue = FromTotalCents(totalCents);
            DollarValueChanged.Invoke(DollarValue);
            _flyValue.ShowFly(other,false);
            ExpensesChanged?.Invoke(ToTotalCents(other));
        }

        public int ToTotalCents(DollarValue dollarValue)
        {
            return dollarValue.Dollars * 100 + dollarValue.Cents;
        }

        public DollarValue FromTotalCents(int totalCents)
        {
            int dollars = totalCents / 100;
            int cents = totalCents % 100;
            return new DollarValue(dollars, cents);
        }
    }
}