using System;
using UnityEngine;

namespace WalletContent
{
    [Serializable]
    public class DollarValue
    {
        [SerializeField]
        private int dollars;
        [SerializeField]
        private int cents;
        
        public int Dollars { get; private set; }
        public int Cents { get; private set; }

        public DollarValue(int dollars, int cents)
        {
            if (cents < 0 || cents > 99)
                throw new ArgumentOutOfRangeException(nameof(cents), "Центы должны быть в диапазоне от 0 до 99.");

            Dollars = dollars;
            Cents = cents;
        }
        
        public int ToTotalCents(DollarValue dollarValue)
        {
            return dollarValue.Dollars * 100 + dollarValue.Cents;
        }
        
        public int ToTotalCents()
        {
            return Dollars * 100 + Cents;
        }
        
        public DollarValue FromTotalCents(int totalCents)
        {
            int dollars = totalCents / 100;
            int cents = totalCents % 100;
            return new DollarValue(dollars, cents);
        }
        
        public override string ToString()
        {
            return $"${Dollars}.{Cents:D2}";
        }
    }
}