using Enums;
using UI.Screens.ShopContent.ShopPages.PageContents.WorksPage;
using UnityEngine;

public class MenuUIItem : MonoBehaviour
{
    [SerializeField] private MenuScrollContent _menuScrollContent;
    [SerializeField] private ItemType _itemType;
    
    public ItemType ItemType => _itemType;
    
    public void RemoveItemToMenu()
    {
        _menuScrollContent.RemoveItem(_itemType);
    }
}