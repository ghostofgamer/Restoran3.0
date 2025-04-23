using Enums;
using SoContent;
using TMPro;
using UI.Screens.ShopContent.ShopPages.PageContents.WorksPage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MenuUIContent
{
    public abstract class AbstractUIMenuItem : MonoBehaviour
    {
        [SerializeField] protected MenuScrollContent _menuScrollContent;
    
        [SerializeField] private ItemType _itemType;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _nameItemText;
    
        private ItemConfig _itemConfig;
    
        public ItemType ItemType => _itemType;
    
        public void Init(ItemsConfig itemsConfig)
        {
            _itemConfig = itemsConfig.GetItemConfig(_itemType);

            if (_itemConfig != null)
            {
                _image.sprite = _itemConfig.Sprite;
                _nameItemText.text = _itemConfig.ItemName;
            }
            else
            {
                Debug.Log("Отсутсвует конфиг ");
            }
        }
    }
}