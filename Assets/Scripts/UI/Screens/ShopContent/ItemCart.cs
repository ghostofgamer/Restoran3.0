using Enums;
using SettingsContent.SoundContent;
using TMPro;
using UI.Screens.ShopContent.ShopPages.PageContents.ProductsPage;
using UnityEngine;
using WalletContent;

namespace UI.Screens.ShopContent
{
    public class ItemCart : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amount;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _name;

        public DollarValue PricePerUnit{ get; private set; }
        
        public int CurrentAmount { get; private set; }
        
        public ItemType ItemType { get; private set; }
        
        public DollarValue TotalPrice{ get; private set; }
        
        private ItemCartScroll _itemCartScroll;
        
        public void Init(ItemType itemType, int amount,DollarValue pricePerUnit,DollarValue totalPrice,string name,
            ItemCartScroll itemCartScroll)
        {
            ItemType = itemType;
            CurrentAmount = amount;
            PricePerUnit = pricePerUnit;
            _amount.text = CurrentAmount.ToString();
            TotalPrice = totalPrice;
            _priceText.text = TotalPrice.ToString();
            _name.text = name;
            _itemCartScroll = itemCartScroll;
        }
        
        public void UpdateAmount(int amount, DollarValue totalPrice)
        {
            CurrentAmount = amount;
            TotalPrice = totalPrice;
            _amount.text = CurrentAmount.ToString();
            _priceText.text = TotalPrice.ToString();
        }
        
        public void IncreaseAmount()
        {
            SoundPlayer.Instance.PlayButtonClick();
            
            if (CurrentAmount >= 9)
                return;

            CurrentAmount++;
            _itemCartScroll.UpdateItemCartInfo(this);
        }

        public void DecreaseAmount()
        {
            SoundPlayer.Instance.PlayButtonClick();
            
            if (CurrentAmount > 1)
            {
                CurrentAmount--;
                _itemCartScroll.UpdateItemCartInfo(this);
            }
        }
    }
}