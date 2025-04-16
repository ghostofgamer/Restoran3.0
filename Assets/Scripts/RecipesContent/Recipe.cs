using System.Collections.Generic;
using Enums;

namespace RecipesContent
{
    [System.Serializable]
    public class Recipe
    {
        public List<ItemType> ItemTypes;
        public ItemType BurgerType;
    }
}