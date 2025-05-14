using System;
using System.IO;
using ADSContent;
using CoppraGames;
using DailyTimerContent;
using PlayerContent.LevelContent;
using TMPro;
using UI.Screens;
using UnityEngine;
using WalletContent;

namespace FortuneContent
{
    public class Fortune : MonoBehaviour
    {
        [SerializeField] private TMP_Text _spinValueText;
        [SerializeField] private SpinWheelController _spinWheelController;
        [SerializeField] private DailyTimerFortune _dailyTimerFortune;
        [SerializeField] private DailyTimerFortune _dailyTimerADSFortune;
        [SerializeField] private GameObject[] _spinButtons;
        [SerializeField] private GameObject _spinFreeButton;
        [SerializeField] private GameObject _spinValueButton;
        [SerializeField] private GameObject _spinADSButton;
        [SerializeField] private GameObject _backWinText;
        [SerializeField] private TMP_Text _prizeText;
        [SerializeField] private ADS _ads;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Animator _fortuneButton;
        [SerializeField] private FortuneScreen _fortuneScreen;

        private int _currentValueSpin = 1;
        private string filePath;

        private Prize[] prizeMap = new Prize[]
        {
            new Prize { Type = PrizesFortune.Money, Value = 10 },
            new Prize { Type = PrizesFortune.Spin, Value = 10 },
            new Prize { Type = PrizesFortune.Money, Value = 50 },
            new Prize { Type = PrizesFortune.XP, Value = 20 },
            new Prize { Type = PrizesFortune.Spin, Value = 3 },
            new Prize { Type = PrizesFortune.XP, Value = 600 },
            new Prize { Type = PrizesFortune.Money, Value = 1000 },
            new Prize { Type = PrizesFortune.XP, Value = 75 },
            new Prize { Type = PrizesFortune.Spin, Value = 2 },
            new Prize { Type = PrizesFortune.XP, Value = 1500 },
        };

        public event Action FreeSpinDayCompleted;

        private void OnEnable()
        {
            _spinWheelController.PrizeCompleted += SetPrize;
            _dailyTimerFortune.TimeOverCompleted += ActivateFreeSpinButton;
            _dailyTimerFortune.TimeNotOverCompleted += ActiveOtherSpinButton;
            _fortuneScreen.FortuneScreenClosed += AnimateButton;
        }

        private void OnDisable()
        {
            _spinWheelController.PrizeCompleted -= SetPrize;
            _dailyTimerFortune.TimeOverCompleted -= ActivateFreeSpinButton;
            _dailyTimerFortune.TimeNotOverCompleted -= ActiveOtherSpinButton;
            _fortuneScreen.FortuneScreenClosed -= AnimateButton;
        }

        private void Start()
        {
            _dailyTimerFortune.UpdateInfo();
            filePath = Path.Combine(Application.persistentDataPath, "spinData.json");
            LoadSpinData();
            _spinValueText.text = $"BALANCE: {_currentValueSpin.ToString()}";
            AnimateButton();
        }

        public void AddSpins(int value)
        {
            _currentValueSpin += value;
            _dailyTimerFortune.UpdateInfo();
            _spinValueText.text = $"BALANCE: {_currentValueSpin.ToString()}";
            SaveSpinData();
        }

        public void OnShow()
        {
            _fortuneScreen.OpenScreen();
            _spinValueText.text = $"BALANCE: {_currentValueSpin.ToString()}";
        }

        public void SpinWheel()
        {
            if (_currentValueSpin <= 0)
                return;

            if (_spinWheelController.IsStarted)
                return;

            _currentValueSpin--;
            SaveSpinData();
            Spin();
            _spinValueText.text = $"BALANCE: {_currentValueSpin.ToString()}";
        }

        public void SpinFree()
        {
            if (_spinWheelController.IsStarted)
                return;

            _dailyTimerFortune.StartButtonClick();
            FreeSpinDayCompleted?.Invoke();
            Spin();
        }

        public void SpinADS()
        {
            if (_spinWheelController.IsStarted)
                return;

            _dailyTimerADSFortune.StartButtonClick();

            _ads.ShowRewarded(() => { Spin(); });
        }

        private void AnimateButton()
        {
            if (_spinFreeButton.activeSelf)
            {
                _fortuneButton.enabled = true;
            }
            else
            {
                _fortuneButton.enabled = false;
                _fortuneButton.transform.localScale = Vector3.one;
            }
        }

        private void Spin()
        {
            _backWinText.SetActive(false);
            _spinWheelController.TurnWheel();
            _dailyTimerFortune.UpdateInfo();
        }

        private void ActivateFreeSpinButton()
        {
            Debug.Log("ActivateFreeSpinButton");
            DeactivationButtons();
            _spinFreeButton.SetActive(true);
        }

        private void ActiveOtherSpinButton()
        {
            Debug.Log("ActiveOtherSpinButton");
            DeactivationButtons();

            if (_currentValueSpin <= 0)
                _dailyTimerADSFortune.UpdateInfo();
            else
                _spinValueButton.SetActive(true);
        }

        private void DeactivationButtons()
        {
            foreach (var button in _spinButtons)
                button.SetActive(false);
        }

        private void SetPrize(int index)
        {
            Debug.Log(index);

            Prize prize = prizeMap[index];
            _backWinText.SetActive(true);
            _prizeText.text = $"You Win: + {prize.Value}  {prize.Type.ToString()}";

            switch (prize.Type)
            {
                case PrizesFortune.Money:
                    _wallet.Add(new DollarValue(prize.Value, 0));
                    break;

                case PrizesFortune.XP:
                    _playerLevel.AddExp(prize.Value);
                    break;

                case PrizesFortune.Spin:
                    AddSpins(prize.Value);
                    break;
            }
        }

        private void SaveSpinData()
        {
            SpinData data = new SpinData { currentValueSpin = _currentValueSpin };
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, json);
            Debug.Log("сохрпанение " + data.currentValueSpin);
        }

        private void LoadSpinData()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                SpinData data = JsonUtility.FromJson<SpinData>(json);
                _currentValueSpin = data.currentValueSpin;
                Debug.Log("Загрузка " + data.currentValueSpin);
            }
        }
    }

    [System.Serializable]
    public class SpinData
    {
        public int currentValueSpin;
    }

    public struct Prize
    {
        public PrizesFortune Type;
        public int Value;
    }
}