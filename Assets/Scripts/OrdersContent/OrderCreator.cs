using System.Collections.Generic;
using Enums;
using RestorantContent;
using UnityEngine;

namespace OrdersContent
{
    public class OrderCreator : MonoBehaviour
    {
        [SerializeField] private Restorant _restorant;
        [SerializeField] private MenuCounter _menuCounter;

        private List<ItemType> _cachedBurgers;
        private List<ItemType> _cachedDrinks;
        private List<ItemType> _cachedExtras;

        [ContextMenu("CreateOrder")]
        public void CreateOrder()
        {
            _cachedBurgers = _menuCounter.GetBurgers();
            _cachedDrinks = _menuCounter.GetDrinks();
            _cachedExtras = _menuCounter.GetExtras();

            Debug.Log($"CreateOrder Burgers {_cachedBurgers.Count} ," +
                      $" drinks {_cachedDrinks.Count} ," +
                      $" extras {_cachedExtras.Count}");

            /*if (_cachedBurgers.Count > 0)
            {
                int randomIndex = Random.Range(0, _cachedBurgers.Count);
                ItemType burgerType = _cachedBurgers[randomIndex];
                Debug.Log($"а закажука я {burgerType}");
            }
            else
            {
                Debug.Log("!В меню нету бургеров");
            }

            if (_cachedDrinks.Count > 0)
            {
                int randomIndex = Random.Range(0, _cachedDrinks.Count);
                ItemType burgerType = _cachedDrinks[randomIndex];
                Debug.Log($"а закажука я {burgerType}");
            }
            else
            {
                Debug.Log("!В меню нету попить");
            }*/

            ItemType burgerType = GetRandomItemType(_cachedBurgers, "бургеров");
            
            if (burgerType != ItemType.Empty)
            {
                Debug.Log($"а закажука я {burgerType}");
            }

            ItemType drinkType = GetRandomItemType(_cachedDrinks, "попить");
            
            if (drinkType != ItemType.Empty)
            {
                Debug.Log($"а закажука я {drinkType}");
            }

            ItemType extraType = GetRandomItemType(_cachedExtras, "допов");
            
            if (extraType != ItemType.Empty)
            {
                Debug.Log($"а закажука я {extraType}");
            }
        }

        private ItemType GetRandomItemType(List<ItemType> itemList, string itemName)
        {
            if (itemList.Count > 0)
            {
                int randomIndex = Random.Range(0, itemList.Count);
                return itemList[randomIndex];
            }
            else
            {
                Debug.Log($"!В меню нету {itemName}");
                return ItemType.Empty;
            }
        }
    }
}