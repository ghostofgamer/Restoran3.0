using Enums;
using UI.Screens.ShopContent.ShopPages.PageContents.WorksPage;
using UnityEngine;

public class DishesUIItem : MonoBehaviour
{
    [SerializeField] private MenuScrollContent _menuScrollContent;
    [SerializeField] private ItemType _itemType;

    public ItemType ItemType => _itemType;
    
    public void AddItemToMenu()
    {
        _menuScrollContent.AddItem(_itemType);
    }
}