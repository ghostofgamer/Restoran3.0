using Enums;
using UI.Screens.ShopContent.ShopPages.PageContents.WorksPage;
using UnityEngine;

public class MenuActivator : MonoBehaviour
{
    [SerializeField] private MenuScrollContent _menuScrollContent;

    private void Start()
    {
        _menuScrollContent.Init();
        _menuScrollContent.AddItem(ItemType.FinishSmallBurger);
        _menuScrollContent.AddItem(ItemType.Coffee);
        /*_menuScrollContent.AddItem(ItemType.FinishCheeseburger);
        _menuScrollContent.AddItem(ItemType.FinishMiddleBurger);*/
    }
}
