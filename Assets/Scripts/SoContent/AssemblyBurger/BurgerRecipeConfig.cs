using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace SoContent.AssemblyBurger
{
    [CreateAssetMenu(fileName = "BurgerRecipeConfig", menuName = "Configs/BurgerRecipeConfig", order = 1)]
    public class BurgerRecipeConfig : ScriptableObject
    {
        public List<Recipes> recipes;
    }
    
    [System.Serializable]
    public class Recipes
    {
        public List<ItemType> ItemTypes;
        public ItemType BurgerType;
    }
}