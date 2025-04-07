using UI.Screens.ShopContent;
using UnityEngine;

namespace UI.Buttons
{
    public class ShopButton : AbstractButton
    {
        [SerializeField] private ShopScreen _shopScreen;

        public override void OnClick()
        {
            _shopScreen.OpenScreen();
        }
    }
}