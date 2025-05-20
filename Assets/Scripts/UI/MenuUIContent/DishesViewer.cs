using TMPro;
using UnityEngine;
using WalletContent;

namespace UI.MenuUIContent
{
    public class DishesViewer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _requiredText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _profitText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private DishesUIItem _dishesUIItem;
        
        private void OnEnable()
        {
            _dishesUIItem.ChangeCurrentPrice += ShowCurrentPrice;
            _dishesUIItem.ChangeProfitPrice += ShowProfit;
            _dishesUIItem.InitCompleted += InitBaseInfo;
        }

        private void OnDisable()
        {
            _dishesUIItem.ChangeCurrentPrice -= ShowCurrentPrice;
            _dishesUIItem.ChangeProfitPrice -= ShowProfit;
            _dishesUIItem.InitCompleted -= InitBaseInfo;
        }

        private void ShowProfit(DollarValue valueProfit)
        {
            Debug.Log("ShowProfit " +valueProfit );
            
            _profitText.text = $"Profit: {valueProfit.ToString()}";
        }

        private void ShowCurrentPrice(DollarValue valueProfit,Color color)
        {
            Debug.Log("ShowCurrentPrice " +valueProfit );
            Debug.Log(color);
            _priceText.text = $"Price {valueProfit}";
            _priceText.color = color;
        }

        private void InitBaseInfo(string requiredInfo, DollarValue costValue)
        {
            _requiredText.text = requiredInfo;
            _costText.text = costValue.ToString();
        }
    }
}