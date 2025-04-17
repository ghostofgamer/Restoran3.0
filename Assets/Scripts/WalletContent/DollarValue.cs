using System;

namespace WalletContent
{
    public class DollarValue
    {
        public int Dollars { get; private set; }
        public int Cents { get; private set; }
        
        public DollarValue(int dollars, int cents)
        {
            if (cents < 0 || cents > 99)
                throw new ArgumentOutOfRangeException(nameof(cents), "Центы должны быть в диапазоне от 0 до 99.");

            Dollars = dollars;
            Cents = cents;
        }
    }
}