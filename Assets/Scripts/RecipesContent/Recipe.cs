using System.Collections.Generic;

namespace RecipesContent
{
    [System.Serializable]
    public class Recipe
    {
        public List<ItemType> ItemTypes;
        public ItemType BurgerType;
    }
}