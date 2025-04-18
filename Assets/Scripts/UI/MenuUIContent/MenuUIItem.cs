namespace UI.MenuUIContent
{
    public class MenuUIItem : AbstractUIMenuItem
    {
        public void RemoveItemToMenu()
        {
            _menuScrollContent.RemoveItem(ItemType);
        }
    }
}