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

            if (_cachedBurgers.Count > 0)
            {
                int randomIndex = Random.Range(0, _cachedBurgers.Count);
                ItemType burgerType = _cachedBurgers[randomIndex];
                Debug.Log($"а закажука я {burgerType}");
            }
            else
            {
                Debug.Log("!В меню нету бургеров");
            }
        }
    }
}