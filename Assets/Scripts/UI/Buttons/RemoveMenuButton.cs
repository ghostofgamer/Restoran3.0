using UI.MenuUIContent;
using UnityEngine;

namespace UI.Buttons
{
    public class RemoveMenuButton : AbstractButton
    {
        [SerializeField] private MenuUIItem _menuUIItem;
        
        public override void OnClick()
        {
            _menuUIItem.RemoveItemToMenu();
        }
    }
}