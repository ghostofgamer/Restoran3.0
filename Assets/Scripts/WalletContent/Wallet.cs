using System;
using UnityEngine;

namespace WalletContent
{
    public class Wallet : MonoBehaviour
    {
        private DollarValue _dollarValue;

        public event Action<DollarValue> DollarValueChanged;

        private void Start()
        {
            _dollarValue = new DollarValue(100, 10);
            DollarValueChanged.Invoke(_dollarValue);
        }

        public void AddTest()
        {
            Add(new DollarValue(1003,65));
        }

        public void SubtractTest()
        {
            Subtract(new DollarValue(135, 06));
        }
        
        public void Add(DollarValue other)
        {
            int totalCents = ToTotalCents(_dollarValue) + ToTotalCents(other);
            _dollarValue= FromTotalCents(totalCents);
            DollarValueChanged.Invoke(_dollarValue);
        }
       
        public void Subtract(DollarValue other)
        {
            int totalCents = ToTotalCents(_dollarValue) - ToTotalCents(other);
            _dollarValue= FromTotalCents(totalCents);
            DollarValueChanged.Invoke(_dollarValue);
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