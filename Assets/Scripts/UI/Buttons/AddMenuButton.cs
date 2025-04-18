using UI.MenuUIContent;
using UnityEngine;

namespace UI.Buttons
{
    public class AddMenuButton :AbstractButton
    {
        [SerializeField] private DishesUIItem _dishesUIItem;
    
        public override void OnClick()
        {
            _dishesUIItem.AddItemToMenu();
        }
    }
}