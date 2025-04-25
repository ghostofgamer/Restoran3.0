using System.Linq;
using AssemblyBurgerContent;
using Enums;
using OrdersContent;
using SoContent;
using SoContent.AssemblyBurger;
using UnityEngine;
using UnityEngine.UI;

public class IngridientPromt : MonoBehaviour
{
    [SerializeField] private AssemblyBurger _assemblyBurger;
    [SerializeField] private Image[] _ingredients;
    [SerializeField] private BurgerRecipeConfig _burgerRecipeConfig;
    [SerializeField] private IngredientsConfig _ingredientsConfig;

    private Recipes _recipes;

    private void OnEnable()
    {
        _assemblyBurger.StackChanged += CheckIngredientsProgress;
    }

    private void OnDisable()
    {
        _assemblyBurger.StackChanged -= CheckIngredientsProgress;
    }

    public void SetIngredients(Order order)
    {
        foreach (var ingredient in _ingredients)
            ingredient.gameObject.SetActive(false);

        if (order.BurgerItemOrder != ItemType.Empty)
        {
            _recipes = _burgerRecipeConfig.GetRecipeByBurgerType(order.BurgerItemOrder);

            for (int i = _recipes.ItemTypes.Count - 1, j = 0; i >= 0; i--, j++)
            {
                _ingredients[j].sprite = _ingredientsConfig.GetSprite(_recipes.ItemTypes[i]);
                _ingredients[j].gameObject.SetActive(true);
            }
        }
    }

    public void CheckIngredientsProgress()
    {
        foreach (var ingredient in _ingredients)
            ingredient.color = Color.white;

        Debug.Log("Стек меняется");
        if (_assemblyBurger.IngredientStack.Count == 0)
            return;

        if (_assemblyBurger.IngredientStack.Count > _recipes.ItemTypes.Count)
        {
            foreach (var ingredient in _ingredients)
                ingredient.color = Color.gray;

            return;
        }

        var stackItems = _assemblyBurger.IngredientStack.Select(x => x.item.ItemType).ToList();
        stackItems.Reverse();

        for (int i = 0; i < _ingredients.Length; i++)
        {
            int recipeIndex = _recipes.ItemTypes.Count - 1 - i;

            /*for (int j = 0; j < stackItems.Count; j++)
                Debug.Log("stack " + j + stackItems[j]);*/

            if (i >= stackItems.Count)
            {
                // _ingredients[i].color = Color.white;
                Debug.Log($"{i}). Индекс выходит за пределы стека");
                continue;
            }

            Debug.Log($"{i}). +  {stackItems[i]}");
            Debug.Log($"{i}). + {_recipes.ItemTypes[recipeIndex]} ");
            Debug.Log($" НОВОЕ {i}). + {_recipes.ItemTypes[recipeIndex]} +///+ {stackItems[stackItems.Count - 1 - i]}");

            if (stackItems[i] == _recipes.ItemTypes[recipeIndex])
            {
                Debug.Log("Правильно ");
                _ingredients[i].color = Color.gray;
            }
            else
            {
                Debug.Log("Не то ");

                /*for (int j = 0; j < _ingredients.Length; j++)
                {
                    _ingredients[j].color = Color.gray;
                    Debug.Log("сколько выключаем " + j);
                }*/

                foreach (var ingredient in _ingredients)
                {
                    ingredient.color = Color.gray;
                    Debug.Log("выключаем ");
                }

                // _ingredients[i].color = Color.white;
            }
        }
    }
}