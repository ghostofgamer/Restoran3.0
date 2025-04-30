using System;
using UnityEngine;

namespace WalletContent
{
    public class Wallet : MonoBehaviour
    {
        public DollarValue DollarValue { get; private set; }

        public event Action<DollarValue> DollarValueChanged;

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
        }

        public void Subtract(DollarValue other)
        {
            int totalCents = ToTotalCents(DollarValue) - ToTotalCents(other);
            DollarValue = FromTotalCents(totalCents);
            DollarValueChanged.Invoke(DollarValue);
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