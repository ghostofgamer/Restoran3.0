using System.Collections.Generic;
using Enums;
using RestorantContent;
using SoContent;
using UI.MenuUIContent;
using UnityEngine;

namespace UI.Screens.ShopContent.ShopPages.PageContents.WorksPage
{
    public class MenuScrollContent : PageScrollContent
    {
        [SerializeField] private MenuCounter _menuCounter;
        [SerializeField] private DishesUIItem[] _dishesUIItems;
        [SerializeField] private MenuUIItem[] _menuUIItems;
        [SerializeField] private ItemsConfig _itemsConfig;

        private Dictionary<ItemType, DishesUIItem> _dishesDictionary;
        private Dictionary<ItemType, MenuUIItem> _menuDictionary;

        public MenuCounter MenuCounter => _menuCounter;

        private void Awake()
        {
            Init();
        }

        public override void Init()
        {
            _dishesDictionary = new Dictionary<ItemType, DishesUIItem>();
            _menuDictionary = new Dictionary<ItemType, MenuUIItem>();

            foreach (var dish in _dishesUIItems)
            {
                _dishesDictionary[dish.ItemType] = dish;
                dish.Init(_itemsConfig);
            }

            foreach (var menuItem in _menuUIItems)
            {
                _menuDictionary[menuItem.ItemType] = menuItem;
                menuItem.Init(_itemsConfig);
            }
        }

        public void AddItem(ItemType type)
        {
            _menuCounter.AddItem(type);
            ChangeActiveItemsList(false, type);
        }

        public void RemoveItem(ItemType type)
        {
            _menuCounter.RemoveItem(type);
            ChangeActiveItemsList(true, type);
        }

        private void ChangeActiveItemsList(bool value, ItemType type)
        {
            if (_dishesDictionary.TryGetValue(type, out var dishItem))
                dishItem.gameObject.SetActive(value);

            if (_menuDictionary.TryGetValue(type, out var menuItem))
                menuItem.gameObject.SetActive(!value);
        }
    }
}