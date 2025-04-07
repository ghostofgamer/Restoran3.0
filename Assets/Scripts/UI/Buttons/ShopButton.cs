using UI.Buttons;
using UI.Screens;
using UnityEngine;

public class ShopButton : AbstractButton
{
    [SerializeField] private ShopScreen _shopScreen;

    public override void OnClick()
    {
        _shopScreen.Open();
    }
}