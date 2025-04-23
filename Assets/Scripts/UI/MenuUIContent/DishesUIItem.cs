namespace UI.MenuUIContent
{
    public class DishesUIItem : AbstractUIMenuItem
    {
        public void AddItemToMenu()
        {
            _menuScrollContent.AddItem(ItemType);
        }
    }
}