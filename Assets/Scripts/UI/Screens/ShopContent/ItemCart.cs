using Enums;
using TMPro;
using UnityEngine;
using WalletContent;

namespace UI.Screens.ShopContent
{
    public class ItemCart : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amount;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _name;

        private int _currentAmount;
        private DollarValue _pricePerUnit;
        
        public ItemType ItemType { get; private set; }

        public void Init(ItemType itemType, int amount,DollarValue pricePerUnit,DollarValue totalPrice,string name )
        {
            ItemType = itemType;
            _currentAmount = amount;
            _pricePerUnit = pricePerUnit;
            _amount.text = _currentAmount.ToString();
            _priceText.text = totalPrice.ToString();
            _name.text = name;
        }
    }
}