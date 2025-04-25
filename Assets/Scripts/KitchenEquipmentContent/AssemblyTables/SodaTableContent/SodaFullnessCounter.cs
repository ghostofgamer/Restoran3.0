using Enums;
using ItemContent;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenEquipmentContent.AssemblyTables.SodaTableContent
{
    public class SodaFullnessCounter : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private Image _imageFullness;
        
        private int _maxFullness = 100;

        public ItemType ItemType => _itemType;
        public int CurrentFullness { get; private set; }

        private void Start()
        {
            CurrentFullness = 50;
            UpdateFillAmount();
        }
        
        public void UseSoda()
        {
            if (CurrentFullness >= 10)
            {
                CurrentFullness -= 10;
                UpdateFillAmount();
            }
            else
            {
                Debug.Log("Недостаточно кофе в кофемашине.");
            }
        }
        
        public void RefillSoda(ItemDrinkPackage itemDrinkPackage)
        {
            int neededSoda = _maxFullness - CurrentFullness;
            int sodaToAdd = Mathf.Min(neededSoda, itemDrinkPackage.CurrentFullness);

            CurrentFullness += sodaToAdd;
            itemDrinkPackage.PourOut(sodaToAdd);
            // coffeeInPacket -= coffeeToAdd;

            UpdateFillAmount();
            Debug.Log($"Кофемашина заполнена. Осталось в пачке: {itemDrinkPackage.CurrentFullness}");
        }
        
        private void UpdateFillAmount()
        {
            _imageFullness.fillAmount = (float)CurrentFullness / _maxFullness;
        }
    }
}