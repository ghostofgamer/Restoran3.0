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
        _menuScrollContent.AddItem(ItemType.FinishCheeseburger);
        _menuScrollContent.AddItem(ItemType.FinishMiddleBurger);
        _menuScrollContent.AddItem(ItemType.FinishStarBurger);
        _menuScrollContent.AddItem(ItemType.FinishBigBurger);
        _menuScrollContent.AddItem(ItemType.FinishMegaBurger);
        _menuScrollContent.AddItem(ItemType.Coffee);
        _menuScrollContent.AddItem(ItemType.SodaBarberry);
        _menuScrollContent.AddItem(ItemType.SodaLemon);
        _menuScrollContent.AddItem(ItemType.SodaOrange);
        _menuScrollContent.AddItem(ItemType.SodaPlum);
        /*_menuScrollContent.AddItem(ItemType.FinishCheeseburger);
        _menuScrollContent.AddItem(ItemType.FinishMiddleBurger);*/
    }
}
