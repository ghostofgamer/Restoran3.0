
using UnityEngine;
using WalletContent;

namespace DailyRewardContent
{
    public class Prize : MonoBehaviour
    {
        [SerializeField] private Wallet _wallet;

        public void Claim(int index)
        {
            switch (index)
            {
                case 0:
                    _wallet.Add(new DollarValue(25,0));
                    break;
                
                case 1:
                    _wallet.Add(new DollarValue(50,0));
                    break;
                
                case 2:
                    _wallet.Add(new DollarValue(75,0));
                    break;
                
                case 3:
                    _wallet.Add(new DollarValue(100,0));
                    break;
                
                case 4:
                    _wallet.Add(new DollarValue(150,0));
                    break;
                
                case 5:
                    _wallet.Add(new DollarValue(200,0));
                    break;
                
                case 6:
                    TakeSuperPrize();
                    break;
            }
        }

        private void TakeSuperPrize()
        {
            /*if (_decorationSystem.GetActivationValueDecoration(_decorationSystem.CurrentDailyRewardDecoration))
            {
                _decorationSystem.ActivateDecoration(_decorationSystem.CurrentDailyRewardDecoration);
                Debug.Log("актвируем ДЕКОР ");
            }
            else
            {
                _currencyController.AddCurrencyFastMoney(CurrencyType.Soft, new(500, 0), true);
                Debug.Log("актвируем БАБКИ ");
            }*/
        }
    }
}