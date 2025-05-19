using TMPro;
using UnityEngine;
using WalletContent;

namespace UI.Screens.ShopContent.WorkersContent
{
    public class WorkerUIViewer : MonoBehaviour
    {
        [SerializeField] private WorkerUIProduct _workerUIProduct;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _salaryText;

        private void OnEnable()
        {
            _workerUIProduct.ValueChanged += SetValue;
        }

        private void OnDisable()
        {
            _workerUIProduct.ValueChanged -= SetValue;
        }

        private void SetValue(DollarValue price, DollarValue salary)
        {
            _priceText.text = price.ToString();
            _salaryText.text = $"Salary: {salary.ToString()}";
        }
    }
}