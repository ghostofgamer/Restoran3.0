using ADSContent;
using EnergyContent;
using Io.AppMetrica;
using UnityEngine;
using WalletContent;

namespace UI.Screens
{
    public class NewLevelUpScreen : AbstractScreen
    {
        [SerializeField] private GameObject _firstScreen;
        [SerializeField] private GameObject _secondScreen;
        [SerializeField] private ADS _ads;
        [SerializeField] private Energy _energy;
        [SerializeField] private Wallet _wallet;

        public override void OpenScreen()
        {
            DeactivateScreens();
            base.OpenScreen();
            _firstScreen.SetActive(true);
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
        }

        public void ChooseDontX2()
        {
            OpenSecondScreen();
            AddPrize(2, 50);
        }

        public void ChooseRewardX2()
        {
            _ads.ShowRewarded(() =>
            {
                OpenSecondScreen();
                AppMetrica.ReportEvent("RewardAD", "{\"" + "ChooseRewardX2UpLevel" + "\":null}");
                AddPrize(4, 100);
            });
        }

        private void OpenSecondScreen()
        {
            _firstScreen.SetActive(false);
            _secondScreen.SetActive(true);
        }

        private void DeactivateScreens()
        {
            _firstScreen.SetActive(false);
            _secondScreen.SetActive(false);
        }

        private void AddPrize(int _energyValue, int _moneyValue)
        {
            _wallet.Add(new DollarValue(_moneyValue, 0));
            _energy.IncreaseEnergy(_energyValue);
        }
    }
}