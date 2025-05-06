using SoContent;
using TMPro;
using UnityEngine;

namespace UI.MenuUIContent
{
    public class DishesUIItem : AbstractUIMenuItem
    {
        [SerializeField] private GameObject _openedContent;
        [SerializeField] private GameObject _closedContent;
        [SerializeField] private TMP_Text _requiredText;

        private int _levelOpened;

        public void AddItemToMenu()
        {
            _menuScrollContent.AddItem(ItemType);
        }

        public override void Init(ItemsConfig itemsConfig)
        {
            base.Init(itemsConfig);

            if (ItemConfig != null)
            {
                _levelOpened = ItemConfig.LevelOpened;
                _requiredText.text = $"Required is {_levelOpened} level";
            }
        }

        public void SetValue(int levelPlayer)
        {
            _openedContent.SetActive(levelPlayer >= _levelOpened);
            _closedContent.SetActive(levelPlayer < _levelOpened);
        }
    }
}