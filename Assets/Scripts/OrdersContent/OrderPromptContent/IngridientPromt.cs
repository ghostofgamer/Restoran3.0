using System.Linq;
using AssemblyBurgerContent;
using Enums;
using OrdersContent;
using SoContent;
using SoContent.AssemblyBurger;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IngridientPromt : MonoBehaviour
{
    [SerializeField] private AssemblyBurger _assemblyBurger;
    [SerializeField] private Image[] _ingredients;
    [SerializeField] private BurgerRecipeConfig _burgerRecipeConfig;
    [SerializeField] private IngredientsConfig _ingredientsConfig;

    private Recipes _recipes;
    private Order _order;

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
        _order = order;

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
        if (_order != null)
        {
            if (_order.IsBurgerCompleted)
            {
                foreach (var ingredient in _ingredients)
                    ingredient.gameObject.SetActive(false);

                if (!_order.IsDrinkCompleted)
                {
                    Debug.Log("_order.DrinkItemOrder " + _order.DrinkItemOrder);
                    Debug.Log("Sprite  " + _ingredientsConfig.GetSprite(_order.DrinkItemOrder));

                    _ingredients[0].sprite = _ingredientsConfig.GetSprite(_order.DrinkItemOrder);
                    _ingredients[0].color = Color.white;
                    _ingredients[0].gameObject.SetActive(true);
                }
            }
            else
                foreach (var ingredient in _ingredients)
                    ingredient.color = Color.white;


            foreach (var ingredient in _ingredients)
                ingredient.transform.localScale = Vector3.one;
        }

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

            if (i >= stackItems.Count)
            {
                Debug.Log($"{i}). Индекс выходит за пределы стека");
                continue;
            }

            if (stackItems[i] == _recipes.ItemTypes[recipeIndex])
            {
                Debug.Log("Правильно ");
                _ingredients[i].color = Color.gray;

                Debug.Log("след ингр " + i + (1));
                Debug.Log("куррент стэцк " + i);
                Debug.Log("стэцк " + stackItems.Count);
                _ingredients[i + 1].transform.localScale = new Vector3(1.3f, 1.3f);
            }
            else
            {
                foreach (var ingredient in _ingredients)
                    ingredient.color = Color.gray;
            }
        }
    }
}