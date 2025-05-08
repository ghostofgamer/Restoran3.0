using System;
using Enums;
using SoContent;
using UnityEngine;
using UnityEngine.UI;
using WalletContent;

namespace UI.Screens.ShopContent.WorkersContent
{
    public class WorkerUIProduct : MonoBehaviour
    {
        public const string Worker = "Worker";

        [SerializeField] private GameObject PayContent;
        [SerializeField] private GameObject UpdateContent;
        [SerializeField] private Image _icon;
        [SerializeField] private WorkerType _workerType;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private GameObject _hireButton;
        [SerializeField] private GameObject _dismissButton;
        [SerializeField] private GameObject _requiredContent;

        private bool isOwned;

        public event Action<DollarValue, DollarValue> ValueChanged;

        public DollarValue Price { get; private set; }
        public DollarValue Salary { get; private set; }

        public WorkerType WorkerType => _workerType;

        private void Start()
        {
            if (PlayerPrefs.GetInt("Zona" + ZoneType.StaffRoom, 0) <= 0)
            {
                ClosePurchased();
                return;
            }

            isOwned = PlayerPrefs.GetInt(Worker + _workerType, 0) > 0;
            SetValue();
        }

        public void Init(WorkerConfig workerConfig)
        {
            Debug.Log("InitWorkersG");

            Price = workerConfig.Price;
            Salary = workerConfig.Salary;
            _icon.sprite = workerConfig.SpriteIcon;

            ValueChanged?.Invoke(Price, Salary);

            if (PlayerPrefs.GetInt("Zona" + ZoneType.StaffRoom, 0) <= 0)
            {
                ClosePurchased();
                return;
            }
            else
            {
                isOwned = PlayerPrefs.GetInt(Worker + _workerType, 0) > 0;
                SetValue();
            }
        }

        public void BuyWorker()
        {
            if (_wallet.DollarValue.ToTotalCents() < Price.ToTotalCents())
            {
                Debug.Log("недостаточно денег");
            }

            _wallet.Subtract(Price);
            isOwned = true;
            PlayerPrefs.SetInt(Worker + _workerType, 1);
            SetValue();
        }

        private void SetValue()
        {
            _requiredContent.SetActive(false);
            PayContent.SetActive(!isOwned);
            UpdateContent.SetActive(isOwned);
            _hireButton.SetActive(!isOwned);
            _dismissButton.SetActive(isOwned);
        }

        public void DismissWorker()
        {
            isOwned = false;
            PlayerPrefs.SetInt(Worker + _workerType, 0);
            SetValue();
        }

        private void ClosePurchased()
        {
            PayContent.SetActive(false);
            UpdateContent.SetActive(false);
            _hireButton.SetActive(false);
            _dismissButton.SetActive(false);
            _requiredContent.SetActive(true);
        }
    }
}