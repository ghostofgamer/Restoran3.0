using I2.Loc;
using SoContent;
using TMPro;
using UnityEngine;
using WalletContent;
using WorkerContent.Upgrade;

namespace UI.Screens.ShopContent.WorkersContent
{
    public class WorkerUIViewer : MonoBehaviour
    {
        [SerializeField] private WorkerUIProduct _workerUIProduct;
        [SerializeField] private WorkerUpgrade _workerUpgrade;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _salaryText;
        [SerializeField] private TMP_Text _workUpgradeText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _priceUpgradeText;

        private void OnEnable()
        {
            _workerUIProduct.ValueChanged += SetValue;
            _workerUIProduct.ParametersValueChanged += ShowInfo;
        }

        private void OnDisable()
        {
            _workerUIProduct.ValueChanged -= SetValue;
            _workerUIProduct.ParametersValueChanged -= ShowInfo;
        }

        private void SetValue(DollarValue price, DollarValue salary)
        {
            _priceText.text = $"{LocalizationManager.GetTermTranslation("Price")}:{price}";
            _salaryText.text = $"{LocalizationManager.GetTermTranslation("Salary")}:{salary}";
        }

        private void ShowInfo(WorkerParameterConfig config)
        {
            _workUpgradeText.text =
                $"{LocalizationManager.GetTermTranslation("Work")} {config.DelayWork.ToString()}s -> <color=green>{config.DelayWorkNext.ToString()}s</color>";
            _levelText.text = $"{LocalizationManager.GetTermTranslation("Level")} {config.Level.ToString()}";
            _priceUpgradeText.text = $"{config.PriceUpgrade.ToString()}";
        }
    }
}